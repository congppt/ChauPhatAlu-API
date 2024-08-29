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

namespace Infrastructure.Implements.Services;

public class ProductService : GenericService<Product>, IProductService
{
    public ProductService(IAppDbContext context, ITimeProvider timeProvider, IClaimProvider claimProvider, IPublishEndpoint publishProducer) : base(context, timeProvider, claimProvider, publishProducer)
    {
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

    public async Task<DetailProductInfo> CreateProductAsync(ProductCreate model)
    {
        var product = model.Adapt<Product>();
        await context.Products.AddAsync(product);
        if (await context.SaveChangesAsync()) return product.Adapt<DetailProductInfo>();
        throw new Exception();
    }
}