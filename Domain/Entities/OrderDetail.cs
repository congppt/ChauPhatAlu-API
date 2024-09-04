using ChauPhatAluminium.Common;

namespace ChauPhatAluminium.Entities;
#pragma warning disable CS8618
public class OrderDetail : BaseEntity
{
    public int OrderId { get; set; }
    public virtual Order Order { get; set; }
    public int ProductId { get; set; }
    public virtual Product Product { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int Height { get; set; }
    public int Width { get; set; }
    public decimal Total { get; set; }
}