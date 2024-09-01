using Application.Models.Common;
using Application.Models.Product;
using ChauPhatAluminium.Entities;
using ChauPhatAluminium.Enums;

namespace Application.Interfaces.Services;

public interface IProductService : IGenericService<Product>
{
    Task<OffsetPage<BasicProductInfo>> GetProductPageAsync(int pageNumber, int pageSize, string? sku, string? name,
        int? brandId, Category? category, int minPrice, int? maxPrice, CancellationToken ct = default);

    Task<DetailProductInfo> GetProductAsync(int productId, CancellationToken ct = default);
    Task<Guid> CreateProductAsync(CreateProduct model);
    Task<Guid> UpdateProductAsync(UpdateProduct model);
}