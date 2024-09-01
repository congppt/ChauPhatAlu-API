using Application.Interfaces.Databases;
using Application.Interfaces.Providers;
using Application.Interfaces.Services;
using Application.Models.Customer;
using Infrastructure.Implements.Databases;
using Infrastructure.Implements.Services;
using Infrastructure.Options;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using RabbitMQ.Client;
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
        services.AddMassTransit(cfg =>
        {
            cfg.SetKebabCaseEndpointNameFormatter();
            cfg.UsingRabbitMq((context, busCfg) =>
            {
                busCfg.Host(config["MessageBroker:RabbitMQ:Host"],config["MessageBroker:RabbitMQ:VirtualHost"], hostCfg =>
                {
                    hostCfg.Username(config["MessageBroker:RabbitMQ:Username"]!);
                    hostCfg.Password(config["MessageBroker:RabbitMQ:Password"]!);
                });
                busCfg.Publish<CreateCustomer>(topo =>
                {
                    topo.ExchangeType = ExchangeType.Direct;
                });
                busCfg.ConfigureEndpoints(context);
            });
        });
        services.AddSingleton<ITimeProvider, TimeProvider>();
        services.AddScoped<IBrandService, BrandService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IProductService, ProductService>();
        return services;
    }
}