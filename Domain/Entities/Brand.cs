using System.ComponentModel.DataAnnotations.Schema;
using ChauPhatAluminium.Common;
using ChauPhatAluminium.Enums;

namespace ChauPhatAluminium.Entities;
#pragma warning disable CS8618
public class Brand : BaseEntity
{
    [Column(TypeName = "citext")]
    public string Name { get; set; }
    public List<Category> Categories { get; set; }
    public virtual HashSet<Product> Products { get; set; }
}