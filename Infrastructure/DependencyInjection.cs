using Application.Interfaces.Databases;
using Application.Interfaces.Providers;
using Application.Interfaces.Services;
using Infrastructure.Implements.Databases;
using Infrastructure.Implements.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using TimeProvider = Infrastructure.Implements.Providers.TimeProvider;
namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<IAppDbContext, AppDbContext>(builder =>
        {
            var dataSource = new NpgsqlDataSourceBuilder(config.GetConnectionString("ChauPhat")).Build();
            builder.UseNpgsql(dataSource, opt => opt.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
        });
        services.AddSingleton<ITimeProvider, TimeProvider>();
        services.AddScoped<IBrandService, BrandService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IProductService, ProductService>();
        return services;
    }
}