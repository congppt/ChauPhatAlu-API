using ChauPhatAluminium.Common;

namespace ChauPhatAluminium.Entities;
#pragma warning disable CS8618
public class District : BaseEntity
{
    public string Name { get; set; }
    public int ProvinceId { get; set; }
    public virtual Province Province { get; set; }
    public virtual HashSet<Ward> Wards { get; set; }
}