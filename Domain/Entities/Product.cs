using System.ComponentModel.DataAnnotations.Schema;
using ChauPhatAluminium.Common;
using ChauPhatAluminium.Enums;

namespace ChauPhatAluminium.Entities;
#pragma warning disable CS8618
public class Product : BaseEntity
{
    [Column(TypeName = "citext")]
    public string Name { get; set; }
    public int BrandId { get; set; }
    public virtual Brand Brand { get; set; }
    public virtual Category Category { get; set; }
    public decimal Price { get; set; }
    public Unit Unit { get; set; }
    [Column(TypeName = "jsonb")]
    public string Description { get; set; }
    public string ImgPath { get; set; }
    public int WarrantyMonth { get; set; }
    public bool IsAvailable { get; set; }
    public virtual HashSet<OrderDetail> OrderDetails { get; set; }
}