using System.Reflection;
using Application.Interfaces.Databases;
using Application.Models.Customer;
using FluentValidation;
using Infrastructure.Implements.Databases;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddDbContext<IAppDbContext, AppDbContext>(dbBuilder =>
{
    var dataSource = new NpgsqlDataSourceBuilder(builder.Configuration.GetConnectionString("ChauPhat")).Build();
    dbBuilder.UseNpgsql(dataSource, opt => opt.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
});
builder.Services.AddMassTransit(cfg =>
{
    cfg.SetKebabCaseEndpointNameFormatter();
    cfg.AddConsumers(Assembly.GetExecutingAssembly());
    cfg.UsingRabbitMq((context, busCfg) =>
    {
        busCfg.Host(builder.Configuration["MessageBroker:RabbitMQ:Host"], builder.Configuration["MessageBroker:RabbitMQ:VirtualHost"], hostCfg =>
        {
            hostCfg.Username(builder.Configuration["MessageBroker:RabbitMQ:Username"]!);
            hostCfg.Password(builder.Configuration["MessageBroker:RabbitMQ:Password"]!);
        });
        busCfg.ConfigureEndpoints(context);
    });
});
builder.Services.AddScoped<IValidator<CreateCustomer>, CreateCustomerValidator>();

var host = builder.Build();
host.Run();