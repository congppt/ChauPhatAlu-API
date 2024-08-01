using ChauPhatAluminium.Common;

namespace ChauPhatAluminium.Entities;
#pragma warning disable CS8618
public class Ward : BaseEntity
{
    public string Name { get; set; }
    public int DistrictId { get; set; }
    public virtual District District { get; set; }
}