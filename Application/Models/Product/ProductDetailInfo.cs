namespace Application.Models.Product;

#pragma warning disable CS8618
public class ProductDetailInfo : ProductBasicInfo
{
    public string Description { get; set; }
    public int WarrantyMonth { get; set; }
    public bool IsAvailable { get; set; }
}