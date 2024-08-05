using Application.Models.Common;
using Application.Models.Product;
using ChauPhatAluminium.Entities;
using ChauPhatAluminium.Enums;

namespace Application.Interfaces.Services;

public interface IProductService : IGenericService<Product>
{
    Task<OffsetPage<ProductBasicInfo>> GetProductPageAsync(int pageNumber, int pageSize, int? brandId, Category? category,
        int minPrice, int? maxPrice);

    Task<ProductDetailInfo> GetProductAsync(int productId);
}