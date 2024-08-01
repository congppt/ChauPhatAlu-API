using ChauPhatAluminium.Common;

namespace ChauPhatAluminium.Entities;
#pragma warning disable CS8618
public class Province : BaseEntity
{
    public string Name { get; set; }
    public virtual HashSet<District> Districts { get; set; }
}