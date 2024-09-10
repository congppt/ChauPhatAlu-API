using System.ComponentModel.DataAnnotations.Schema;
using ChauPhatAluminium.Common;
using ChauPhatAluminium.Enums;
using Microsoft.EntityFrameworkCore;

namespace ChauPhatAluminium.Entities;
#pragma warning disable CS8618
[Index(nameof(Name), IsUnique = true)]
public class Brand : BaseEntity
{
    [Column(TypeName = "citext")]
    public string Name { get; set; }
    public virtual HashSet<Product> Products { get; set; }
}