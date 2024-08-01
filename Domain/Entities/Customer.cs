using System.ComponentModel.DataAnnotations.Schema;
using ChauPhatAluminium.Common;
using Microsoft.EntityFrameworkCore;

namespace ChauPhatAluminium.Entities;
#pragma warning disable CS8618
[Index(nameof(Phone), IsUnique = true)]
public class Customer : BaseEntity
{
    [Column(TypeName = "citext")]
    public string Name { get; set; }
    public string Phone { get; set; }
    public bool IsMale { get; set; }
    public string Address { get; set; }
    public int WardId { get; set; }
    public virtual Ward Ward { get; set; }
}