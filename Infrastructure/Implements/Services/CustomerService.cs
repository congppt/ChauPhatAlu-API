using Application.Interfaces.Databases;
using Application.Interfaces.Providers;
using Application.Interfaces.Services;
using Application.Models.Common;
using ChauPhatAluminium.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implements.Services;
public class CustomerService : GenericService<Customer>, ICustomerService
{
    public CustomerService(IAppDbContext context, ITimeProvider timeProvider, IClaimProvider claimProvider) : base(context, timeProvider, claimProvider)
    {
    }

    public async Task<OffsetPage<Customer>> GetCustomerPageAsync(int pageNumber, int pageSize, string? phone, string? name)
    {
        var source = context.GetUntrackedQuery<Customer>();
        if (!string.IsNullOrWhiteSpace(phone))
            source = source.Where(c => c.Phone.Contains(phone));
        if (!string.IsNullOrWhiteSpace(name))
            source = source.Where(c => EF.Functions.ILike(c.Name, $"%{name}%"));
        return await OffsetPage<Customer>.CreateAsync(source, pageNumber, pageSize);
    }
}