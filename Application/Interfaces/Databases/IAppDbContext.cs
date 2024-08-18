using System.Linq.Expressions;
using ChauPhatAluminium.Common;
using ChauPhatAluminium.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces.Databases;

public interface IAppDbContext
{
    DbSet<Brand> Brands { get; }
    DbSet<Customer> Customers { get; }
    DbSet<Order> Orders { get; }
    DbSet<OrderDetail> OrderDetails { get; }
    DbSet<Product> Products { get; }
    IQueryable<T> GetUntrackedQuery<T>() where T : BaseEntity;
    Task<T?> GetByIdAsync<T>(int id, CancellationToken ct = default, params Expression<Func<T, object>>[] navigations) where T : BaseEntity;
    Task<bool> SaveChangesAsync(CancellationToken ct = default);
}