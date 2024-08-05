using Application.Interfaces.Databases;
using Application.Interfaces.Providers;
using Application.Interfaces.Services;
using Application.Models.Common;
using Application.Models.Product;
using ChauPhatAluminium.Constants;
using ChauPhatAluminium.Entities;
using ChauPhatAluminium.Enums;
using Mapster;

namespace Infrastructure.Implements.Services;

public class ProductService : GenericService<Product>, IProductService
{
    public ProductService(IAppDbContext context, ITimeProvider timeProvider, IClaimProvider claimProvider) : base(context, timeProvider, claimProvider)
    {
    }

    public async Task<OffsetPage<ProductBasicInfo>> GetProductPageAsync(int pageNumber, int pageSize, int? brandId, Category? category, int minPrice, int? maxPrice)
    {
        if (maxPrice < minPrice) throw new ArgumentException();
        var source = context.GetUntrackedQuery<Product>();
        var role = claimProvider.GetClaim(ClaimConstants.Role, Role.Guest);
        if (role == Role.Guest) 
            source = source.Where(p => p.IsAvailable);
        if (brandId != null)
            source = source.Where(p => p.BrandId == brandId);
        if (category.HasValue)
            source = source.Where(p => p.Category == category.Value);
        source = source.Where(p => p.Price >= minPrice);
        if (maxPrice.HasValue)
            source = source.Where(p => p.Price <= maxPrice);
        source = source.OrderByDescending(p => p.Id);
        return await OffsetPage<ProductBasicInfo>.CreateAsync(source.ProjectToType<ProductBasicInfo>(), pageNumber,
            pageSize);
    }

    public async Task<ProductDetailInfo> GetProductAsync(int productId)
    {
        var product = await context.GetByIdAsync<Product>(productId) ?? throw new KeyNotFoundException();
        var role = claimProvider.GetClaim(ClaimConstants.Role, Role.Guest);
        if (!product.IsAvailable && role == Role.Guest) throw new KeyNotFoundException();
        return product.Adapt<ProductDetailInfo>();
    }
}