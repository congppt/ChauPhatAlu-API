using Application.Models.Customer;
using ChauPhatAluminium.Enums;

namespace Application.Models.Order;

#pragma warning disable Cs8618
public class OrderBasicInfo
{
    public int Id { get; set; }
    public virtual CustomerBasicInfo Customer { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal Total { get; set; }
    public OrderStatus Status { get; set; }
}