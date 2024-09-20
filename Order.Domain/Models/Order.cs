using Order.Domain.Abstractions;
using Order.Domain.Enums;

namespace Order.Domain.Models;
#pragma warning disable CS8618
public class Order : Aggregate
{
    public int CustomerId { get; private set; }
    public string Address { get; private set; }
    public OrderStatus Status { get; private set; }
    public decimal Total { get; private set; }
    
}