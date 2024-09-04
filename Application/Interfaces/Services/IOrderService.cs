using Application.Models.Common;
using Application.Models.Order;
using ChauPhatAluminium.Entities;
using ChauPhatAluminium.Enums;

namespace Application.Interfaces.Services;

public interface IOrderService : IGenericService<Order>
{
    Task<OffsetPage<BasicOrderInfo>> GetOrderPageAsync(int pageNumber, int pageSize, OrderStatus? status,
        int? customerId, DateTime? minDate, DateTime? maxDate, CancellationToken ct = default);
    Task<DetailOrderInfo> GetOrderAsync(int orderId, CancellationToken ct = default);
    Task<DetailOrderInfo> CreateOrderAsync(CreateOrder model, CancellationToken ct = default);
}