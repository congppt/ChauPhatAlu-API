using ChauPhatAluminium.Enums;

namespace Application.Models.Brand;

#pragma warning disable CS8618
public class BrandCreate
{
    public string Name { get; set; }
    public List<Category> Categories { get; set; }
}