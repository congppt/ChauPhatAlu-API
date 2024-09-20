using Order.Domain.Abstractions;
using Order.Domain.Enums;
using Order.Domain.Events;

namespace Order.Domain.Models;
#pragma warning disable CS8618
public class Order : Aggregate
{
    public int CustomerId { get; private set; }
    public string Address { get; private set; }
    public OrderStatus Status { get; private set; }
    public decimal Total { get; private set; }
    public virtual HashSet<OrderItem> OrderItems { get; private set; }

    public static Order Create(int customerId, string address)
    {
        var order = new Order
        {
            CustomerId = customerId,
            Address = address,
            Status = OrderStatus.Pending
        };
        order.AddDomainEvent(new OrderCreatedEvent(order));
        return order;
    }

    public void Update(OrderStatus status)
    {
        Status = status;
        AddDomainEvent(new OrderUpdatedEvent(this));
    }

    public void AddItem(Product product, int quantity, int width, int height)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height);
        var item = OrderItem.Create(product, quantity, height, width);
        OrderItems.Add(item);
    }

    public void RemoveItem(int productId)
    {
        var item = OrderItems.FirstOrDefault(item => item.ProductId == productId);
        if (item != null) OrderItems.Remove(item);
    }
}