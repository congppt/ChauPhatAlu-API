using ChauPhatAluminium.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces.Databases;

public interface IAppDbContext
{
    DbSet<Brand> Brands { get; }
    DbSet<Customer> Customers { get; }
    DbSet<District> Districts { get; }
    DbSet<Order> Orders { get; }
    DbSet<OrderDetail> OrderDetails { get; }
    DbSet<Product> Products { get; }
    DbSet<Province> Provinces { get; }
    DbSet<Ward> Wards { get; }
}