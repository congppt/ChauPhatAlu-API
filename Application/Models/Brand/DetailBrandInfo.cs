using ChauPhatAluminium.Enums;

namespace Application.Models.Brand;

#pragma warning disable CS8618
public class DetailBrandInfo : BasicBrandInfo
{
    public List<Category> Categories { get; set; }
}