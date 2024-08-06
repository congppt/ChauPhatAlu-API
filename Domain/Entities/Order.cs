using ChauPhatAluminium.Common;
using ChauPhatAluminium.Enums;

namespace ChauPhatAluminium.Entities;
#pragma warning disable CS8618
public class Order : BaseEntity
{
    public int CustomerId { get; set; }
    public virtual Customer Customer { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal Total { get; set; }
    public OrderStatus Status { get; set; }
    public string Address { get; set; }
    public List<Trace> Traces { get; set; }
    public virtual HashSet<OrderDetail> Details { get; set; }
    public class Trace
    {
        public OrderStatus Status { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string Note { get; set; }
    }
}