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
    Task<Guid> CreateProductAsync(CreateProduct model, CancellationToken ct = default);
    Task<Guid> UpdateProductAsync(UpdateProduct model, CancellationToken ct = default);
    Task<DetailProductInfo> SwitchProductStatusAsync(int id, CancellationToken ct = default);
    Task<string> GetImageUploadLinkAsync(int id);
    Task SetProductImagePathAsync(int id, string imagePath, CancellationToken ct = default);
}