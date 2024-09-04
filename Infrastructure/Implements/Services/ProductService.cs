using Amazon.S3;
using Amazon.S3.Model;
using Application.Interfaces.Databases;
using Application.Interfaces.Providers;
using Application.Interfaces.Services;
using Application.Models.Common;
using Application.Models.Product;
using ChauPhatAluminium.Constants;
using ChauPhatAluminium.Entities;
using ChauPhatAluminium.Enums;
using Mapster;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Implements.Services;

public class ProductService : GenericService<Product>, IProductService
{
    private readonly IAmazonS3 _s3;
    public ProductService(IAppDbContext context, ITimeProvider timeProvider, IClaimProvider claimProvider,
        IPublishEndpoint publishProducer, IAmazonS3 s3, IConfiguration config) : base(context, timeProvider, claimProvider, publishProducer, config)
    {
        _s3 = s3;
    }

    public async Task<OffsetPage<BasicProductInfo>> GetProductPageAsync(int pageNumber, int pageSize, string? sku,
        string? name, int? brandId, Category? category, int minPrice, int? maxPrice, CancellationToken ct = default)
    {
        if (maxPrice < minPrice) throw new ArgumentException();
        var source = context.GetUntrackedQuery<Product>();
        var role = claimProvider.GetClaim(ClaimConstants.Role, Role.Guest);
        if (role == Role.Guest) 
            source = source.Where(p => p.IsAvailable);
        if (!string.IsNullOrEmpty(sku))
            source = source.Where(p => p.SKU.StartsWith(sku));
        if (!string.IsNullOrWhiteSpace(name))
            source = source.Where(p => EF.Functions.ILike(p.Name, $"%{name}%"));
        if (brandId != null)
            source = source.Where(p => p.BrandId == brandId);
        if (category.HasValue)
            source = source.Where(p => p.Category == category.Value);
        source = source.Where(p => p.Price >= minPrice);
        if (maxPrice.HasValue)
            source = source.Where(p => p.Price <= maxPrice);
        source = source.OrderByDescending(p => p.Id);
        return await OffsetPage<BasicProductInfo>.CreateAsync(source.ProjectToType<BasicProductInfo>(), pageNumber,
            pageSize, ct);
    }

    public async Task<DetailProductInfo> GetProductAsync(int productId, CancellationToken ct = default)
    {
        var product = await context.GetByIdAsync<Product>(productId, ct, p => p.Brand) ?? throw new KeyNotFoundException();
        var role = claimProvider.GetClaim(ClaimConstants.Role, Role.Guest);
        if (!product.IsAvailable && role == Role.Guest) throw new KeyNotFoundException();
        return product.Adapt<DetailProductInfo>();
    }

    public async Task<Guid> CreateProductAsync(CreateProduct model)
    {
        await publishProducer.Publish(model);
        return model.Guid;
    }

    public async Task<Guid> UpdateProductAsync(UpdateProduct model)
    {
        await publishProducer.Publish(model);
        return model.Guid;
    }

    public async Task SwitchProductStatusAsync(int id, CancellationToken ct = default)
    {
        var product = await context.GetByIdAsync<Product>(id, ct) ?? throw new KeyNotFoundException();
        product.IsAvailable = !product.IsAvailable;
        await context.SaveChangesAsync(ct);
    }

    public async Task<string> GetImageUploadLinkAsync(int id)
    {
        var objectKey = $"product-images/{id}_"+ Guid.NewGuid() + ".jpg";
        var request = new GetPreSignedUrlRequest
        {
            BucketName = config["AWS:S3:BucketName"],
            Key = objectKey,
            Expires = timeProvider.Now().AddMinutes(5),
            Verb = HttpVerb.PUT,
            ContentType = "image/jpeg",
            
        };
        var url = await _s3.GetPreSignedURLAsync(request);
        return url;
    }

    public async Task SetProductImagePathAsync(int id, string imagePath, CancellationToken ct = default)
    {
        var parameters = imagePath.Split(['/', '_']);
        //if (parameters[0] != DefaultConstants.PRODUCT_IMG_FOLDER) throw new ArgumentException();
        var product = await context.GetByIdAsync<Product>(id, ct) ?? throw new KeyNotFoundException();
        product.ImgPath = imagePath;
        await context.SaveChangesAsync(ct);
        //await uow.ProductRepo.CacheEntityAsync(id, product);
    }
}