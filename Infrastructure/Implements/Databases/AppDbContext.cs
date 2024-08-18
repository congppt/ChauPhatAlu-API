using System.Linq.Expressions;
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
        return Set<T>().AsNoTrackingWithIdentityResolution();
    }

    public async Task<T?> GetByIdAsync<T>(int id, CancellationToken ct = default, params Expression<Func<T, object>>[] navigations) where T : BaseEntity
    {
        var source = Set<T>().AsQueryable();
        source = navigations.Aggregate(source, (current, navigation) => current.Include(navigation));
        return await source.FirstOrDefaultAsync(e => e.Id == id, ct);
    }

    public async Task<bool> SaveChangesAsync(CancellationToken ct = default)
    {
        return await base.SaveChangesAsync(ct) > 0;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.HasPostgresExtension("citext");
        modelBuilder.HasPostgresExtension("unaccent");
    }
}