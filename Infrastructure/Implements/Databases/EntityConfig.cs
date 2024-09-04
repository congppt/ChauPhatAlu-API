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
        };
        var xingfa = new Brand
        {
            Id = 2,
            Name = "Xingfa Quảng Đông",
        };
        var achaudoor = new Brand
        {
            Id = 3,
            Name = "Á Châu Door",
        };
        var titadoor = new Brand
        {
            Id = 4,
            Name = "Titadoor",
        };
        builder.HasData(austdoor, xingfa, achaudoor, titadoor);
    }
}

public class OrderConfig : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.OwnsMany(o => o.Traces, b => b.ToJson());
        //builder.HasMany(o => o.Details).WithOne(o => o.Order).HasForeignKey(o => o.OrderId);
    }
}