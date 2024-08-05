﻿using Application.Interfaces.Databases;
using Application.Interfaces.Providers;
using Application.Interfaces.Services;
using Application.Models.Brand;
using Application.Models.Common;
using ChauPhatAluminium.Entities;
using ChauPhatAluminium.Enums;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implements.Services;

public class BrandService : GenericService<Brand>, IBrandService
{
    public BrandService(IAppDbContext context, ITimeProvider timeProvider, IClaimProvider claimProvider) : base(context, timeProvider, claimProvider)
    {
    }

    public async Task<OffsetPage<BrandBasicInfo>> GetBrandPageAsync(int pageNumber, int pageSize, Category? category, string? name)
    {
        var source = context.GetUntrackedQuery<Brand>();
        if (category.HasValue)
            source = source.Where(b => b.Categories.Contains(category.Value));
        if (!string.IsNullOrWhiteSpace(name))
            source = source.Where(b => EF.Functions.ILike(b.Name, $"%{name}%"));
        source = source.OrderByDescending(b => b.Id);
        return await OffsetPage<BrandBasicInfo>.CreateAsync(source.ProjectToType<BrandBasicInfo>(), pageNumber, pageSize);
    }
}