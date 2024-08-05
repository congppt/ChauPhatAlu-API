using Application.Models.Brand;
using Application.Models.Common;
using ChauPhatAluminium.Entities;
using ChauPhatAluminium.Enums;

namespace Application.Interfaces.Services;

public interface IBrandService : IGenericService<Brand>
{
    Task<OffsetPage<BrandBasicInfo>> GetBrandPageAsync(int pageNumber, int pageSize, Category? category, string? name);
    Task<BrandDetailInfo> GetBrandAsync(int brandId);
}