using Application.Interfaces.Databases;
using Application.Interfaces.Providers;
using Application.Interfaces.Services;
using ChauPhatAluminium.Entities;

namespace Infrastructure.Implements.Services;

public class OrderService : GenericService<Order>, IOrderService
{
    public OrderService(IAppDbContext context, ITimeProvider timeProvider, IClaimProvider claimProvider) : base(context, timeProvider, claimProvider)
    {
    }
}