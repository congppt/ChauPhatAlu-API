using Application.Interfaces.Databases;
using Application.Interfaces.Providers;
using Application.Interfaces.Services;
using Application.Models.Common;
using Application.Models.Order;
using ChauPhatAluminium.Entities;
using ChauPhatAluminium.Enums;
using Mapster;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Implements.Services;

public class OrderService : GenericService<Order>, IOrderService
{
    public OrderService(IAppDbContext context, ITimeProvider timeProvider, IClaimProvider claimProvider,
        IPublishEndpoint publishProducer, IConfiguration config) : base(context, timeProvider, claimProvider,
        publishProducer, config)
    {
    }

    public async Task<OffsetPage<BasicOrderInfo>> GetOrderPageAsync(int pageNumber, int pageSize, OrderStatus? status,
        int? customerId, DateTime? minDate,
        DateTime? maxDate, CancellationToken ct = default)
    {
        var now = timeProvider.Now();
        if (minDate > maxDate) throw new ArgumentException();
        if (maxDate > now) throw new ArgumentException();
        var source = context.GetUntrackedQuery<Order>();
        if (status.HasValue)
            source = source.Where(o => o.Status == status);
        if (customerId.HasValue)
            source = source.Where(o => o.CustomerId == customerId);
        if (minDate.HasValue)
            source = source.Where(o => o.CreatedAt >= minDate);
        if (maxDate.HasValue)
            source = source.Where(o => o.CreatedAt <= maxDate);
        source = source.OrderByDescending(o => o.Id);
        return await OffsetPage<BasicOrderInfo>.CreateAsync(source.ProjectToType<BasicOrderInfo>(), pageNumber,
            pageSize, ct);
    }

    public async Task<DetailOrderInfo> GetOrderAsync(int orderId, CancellationToken ct = default)
    {
        var order = await context.GetUntrackedQuery<Order>().Include(o => o.Details).ThenInclude(o => o.Product)
            .FirstOrDefaultAsync(o => o.Id == orderId, ct) ?? throw new KeyNotFoundException();
        return order.Adapt<DetailOrderInfo>();
    }

    public async Task<Guid> CreateOrderAsync(CreateOrder model, CancellationToken ct = default)
    {
        await publishProducer.Publish(model, ct);
        return model.Guid;
    }

    public async Task<Guid> UpdateOrderStatusAsync(UpdateOrderStatus model, CancellationToken ct = default)
    {
        await publishProducer.Publish(model, ct);
        return model.Guid;
    }
}