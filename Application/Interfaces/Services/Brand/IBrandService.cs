using Application.Interfaces.Services.Generic;
using Application.Models.Brand;
using Application.Models.Common;

namespace Application.Interfaces.Services.Brand;

public interface IBrandService : IGenericService<ChauPhatAluminium.Entities.Brand>
{
    Task<OffsetPage<BasicBrandInfo>> GetBrandPageAsync(int pageNumber, int pageSize, string? name,
        CancellationToken ct = default);
    Task<DetailBrandInfo> GetBrandAsync(int brandId, CancellationToken ct = default);
    Task<Guid> CreateBrandAsync(CreateBrand model, CancellationToken ct = default);
}