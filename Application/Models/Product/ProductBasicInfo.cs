using System.ComponentModel.DataAnnotations.Schema;
using Application.Models.Brand;
using ChauPhatAluminium.Entities;
using ChauPhatAluminium.Enums;

namespace Application.Models.Product;
#pragma  warning disable CS8618
public class ProductBasicInfo
{
    public string Name { get; set; }
    public BrandBasicInfo Brand { get; set; }
    public virtual Category Category { get; set; }
    public decimal Price { get; set; }
    public Unit Unit { get; set; }
    public string ImgPath { get; set; }
    public int WarrantyMonth { get; set; }
}