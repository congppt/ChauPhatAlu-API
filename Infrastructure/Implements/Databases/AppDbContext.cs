using System.Reflection;
using Application.Interfaces.Databases;
using ChauPhatAluminium.Common;
using ChauPhatAluminium.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implements.Databases;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    {
    }
    
    public DbSet<Brand> Brands { get; set; } 
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public DbSet<Product> Products { get; set; }
    public IQueryable<T> GetUntrackedQuery<T>() where T : BaseEntity
    {
        return this.Set<T>().AsNoTrackingWithIdentityResolution();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.HasPostgresExtension("citext");
        modelBuilder.HasPostgresExtension("unaccent");
    }
}