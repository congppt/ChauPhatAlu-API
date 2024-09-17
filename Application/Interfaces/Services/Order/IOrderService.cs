using Application.Interfaces.Services.Generic;
using Application.Models.Common;
using Application.Models.Order;
using ChauPhatAluminium.Enums;

namespace Application.Interfaces.Services.Order;

public interface IOrderService : IGenericService<ChauPhatAluminium.Entities.Order>
{
    Task<OffsetPage<BasicOrderInfo>> GetOrderPageAsync(int pageNumber, int pageSize, OrderStatus? status,
        int? customerId, DateTime? minDate, DateTime? maxDate, CancellationToken ct = default);
    Task<DetailOrderInfo> GetOrderAsync(int orderId, CancellationToken ct = default);
    Task<Guid> CreateOrderAsync(CreateOrder model, CancellationToken ct = default);
    Task<Guid> UpdateOrderStatusAsync(UpdateOrderStatus model, CancellationToken ct = default);
}