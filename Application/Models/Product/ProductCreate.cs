using System.ComponentModel.DataAnnotations.Schema;
using ChauPhatAluminium.Enums;

namespace Application.Models.Product;

#pragma warning disable CS8618
public class ProductCreate
{
    public string Name { get; set; }
    public int BrandId { get; set; }
    public string SKU { get; set; }
    public Category Category { get; set; }
    public decimal Price { get; set; }
    public Unit Unit { get; set; }
    public string Description { get; set; }
    public int WarrantyMonth { get; set; }
    public bool IsAvailable { get; set; }
}