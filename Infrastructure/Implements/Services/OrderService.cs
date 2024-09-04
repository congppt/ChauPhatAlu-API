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

    public async Task<DetailOrderInfo> CreateOrderAsync(CreateOrder model, CancellationToken ct = default)
    {
        var order = model.Adapt<Order>();
        order.Details = [];
        var total = 0m;
        var products = await context.Products.Where(p => model.Products.Keys.Contains(p.Id)).ToListAsync(ct);
        foreach (var product in products)
        {
            var detail = model.Products[product.Id].Adapt<OrderDetail>();
            detail.Product = product;
            detail.Price = product.Price;
            detail.Total = detail.Price * detail.Quantity;
            switch (product.Category)
            {
                case Category.AluminiumDoor:
                case Category.RollingDoor:
                case Category.SlidingDoor:
                    detail.Total *= detail.Height * detail.Width;
                    break;
            }
            total += detail.Total;
            order.Details.Add(detail);
        }
        order.Customer = await context.GetByIdAsync<Customer>(order.CustomerId, ct);
        order.Total = total;
        order.Status = OrderStatus.Processing;
        order.CreatedAt = timeProvider.Now();
        order.Traces = [ new Order.Trace
        {
            ModifiedAt = order.CreatedAt,
            Note = "",
            Status = order.Status
        }];
        await context.Orders.AddAsync(order, ct);
        if (!await context.SaveChangesAsync(ct)) throw new DbUpdateException();
        return order.Adapt<DetailOrderInfo>();
    }
}