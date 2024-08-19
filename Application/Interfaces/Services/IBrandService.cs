using Application.Models.Brand;
using Application.Models.Common;
using ChauPhatAluminium.Entities;
using ChauPhatAluminium.Enums;

namespace Application.Interfaces.Services;

public interface IBrandService : IGenericService<Brand>
{
    Task<OffsetPage<BasicBrandInfo>> GetBrandPageAsync(int pageNumber, int pageSize, string? name,
        CancellationToken ct = default);
    Task<DetailBrandInfo> GetBrandAsync(int brandId, CancellationToken ct = default);
    Task<DetailBrandInfo> CreateBrandAsync(BrandCreate model, CancellationToken ct = default);
}