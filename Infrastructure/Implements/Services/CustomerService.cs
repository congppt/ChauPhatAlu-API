﻿using Application.Interfaces.Databases;
using Application.Interfaces.Providers;
using Application.Interfaces.Services;
using Application.Models.Common;
using Application.Models.Customer;
using ChauPhatAluminium.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implements.Services;
public class CustomerService : GenericService<Customer>, ICustomerService
{
    public CustomerService(IAppDbContext context, ITimeProvider timeProvider, IClaimProvider claimProvider) : base(context, timeProvider, claimProvider)
    {
    }

    public async Task<OffsetPage<CustomerBasicInfo>> GetCustomerPageAsync(int pageNumber, int pageSize, string? phone, string? name)
    {
        var source = context.GetUntrackedQuery<Customer>();
        if (!string.IsNullOrWhiteSpace(phone))
            source = source.Where(c => c.Phone.Contains(phone));
        if (!string.IsNullOrWhiteSpace(name))
            source = source.Where(c => EF.Functions.ILike(c.Name, $"%{name}%"));
        source = source.OrderByDescending(c => c.Id);
        return await OffsetPage<CustomerBasicInfo>.CreateAsync(source.ProjectToType<CustomerBasicInfo>(), pageNumber, pageSize);
    }
}