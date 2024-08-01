using ChauPhatAluminium.Common;

namespace ChauPhatAluminium.Entities;
#pragma warning disable CS8618
public class Order : BaseEntity
{
    public int CustomerId { get; set; }
    public virtual Customer Customer { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal Total { get; set; }
    public virtual HashSet<OrderDetail> Details { get; set; }
}