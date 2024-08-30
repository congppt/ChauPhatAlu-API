using Application.Interfaces.Databases;
using Application.Interfaces.Providers;
using Application.Interfaces.Services;
using Application.Models.Brand;
using Application.Models.Common;
using ChauPhatAluminium.Entities;
using ChauPhatAluminium.Enums;
using Mapster;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implements.Services;

public class BrandService : GenericService<Brand>, IBrandService
{
    public BrandService(IAppDbContext context, ITimeProvider timeProvider, IClaimProvider claimProvider,
        IPublishEndpoint publishProducer) : base(context, timeProvider, claimProvider, publishProducer)
    {
    }

    public async Task<OffsetPage<BasicBrandInfo>> GetBrandPageAsync(int pageNumber, int pageSize, string? name, CancellationToken ct = default)
    {
        var source = context.GetUntrackedQuery<Brand>();
        if (!string.IsNullOrWhiteSpace(name))
            source = source.Where(b => EF.Functions.ILike(b.Name, $"%{name}%"));
        source = source.OrderByDescending(b => b.Id);
        return await OffsetPage<BasicBrandInfo>.CreateAsync(source.ProjectToType<BasicBrandInfo>(), pageNumber, pageSize, ct);
    }

    public async Task<DetailBrandInfo> GetBrandAsync(int brandId, CancellationToken ct = default)
    {
        var brand = await context.GetByIdAsync<Brand>(brandId, ct) ?? throw new KeyNotFoundException();
        return brand.Adapt<DetailBrandInfo>();
    }

    public async Task<DetailBrandInfo> CreateBrandAsync(CreateBrand model, CancellationToken ct = default)
    {
        var brand = model.Adapt<Brand>();
        await context.Brands.AddAsync(brand, ct);
        if (await context.SaveChangesAsync(ct)) return brand.Adapt<DetailBrandInfo>();
        throw new Exception();
    }
}