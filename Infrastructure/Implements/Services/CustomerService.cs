using Application.Interfaces.Databases;
using Application.Interfaces.Providers;
using Application.Interfaces.Services;
using ChauPhatAluminium.Entities;

namespace Infrastructure.Implements.Services;
public class CustomerService : GenericService<Customer>, ICustomerService
{
    public CustomerService(IAppDbContext context, ITimeProvider timeProvider, IClaimProvider claimProvider) : base(context, timeProvider, claimProvider)
    {
    }
}