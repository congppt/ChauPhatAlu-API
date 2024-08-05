using ChauPhatAluminium.Enums;

namespace Application.Models.Brand;

#pragma warning disable CS8618
public class BrandDetailInfo : BrandBasicInfo
{
    public List<Category> Categories { get; set; }
}