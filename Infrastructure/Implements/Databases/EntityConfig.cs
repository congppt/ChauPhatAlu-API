using ChauPhatAluminium.Entities;
using ChauPhatAluminium.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Implements.Databases;

public class BrandConfig : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        var austdoor = new Brand
        {
            Id = 1,
            Name = "Austdoor",
            Categories = [Category.Motor, Category.RollingDoor, Category.Remote, Category.UPS]
        };
        var xingfa = new Brand
        {
            Id = 2,
            Name = "Xingfa Quảng Đông",
            Categories = [Category.AluminiumDoor]
        };
        var achaudoor = new Brand
        {
            Id = 3,
            Name = "Á Châu Door",
            Categories = [Category.RollingDoor, Category.UPS, Category.SlidingDoor, Category.Motor,]
        };
        var titadoor = new Brand
        {
            Id = 4,
            Name = "Titadoor",
            Categories = [Category.RollingDoor]
        };
        builder.HasData(austdoor, xingfa, achaudoor, titadoor);
    }
}

public class OrderConfig : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.OwnsMany(o => o.Traces, b => b.ToJson());
    }
}