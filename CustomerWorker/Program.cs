using System.Reflection;
using Application.Interfaces.Databases;
using Application.Interfaces.Providers;
using Infrastructure.Implements.Databases;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<IAppDbContext, AppDbContext>(dbBuilder =>
{
    var dataSource = new NpgsqlDataSourceBuilder(builder.Configuration.GetConnectionString("ChauPhat")).Build();
    dbBuilder.UseNpgsql(dataSource, opt => opt.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
});
builder.Services.AddMassTransit(cfg =>
{
    cfg.SetKebabCaseEndpointNameFormatter();
    cfg.UsingRabbitMq((context, busCfg) =>
    {
        busCfg.Host(new Uri(builder.Configuration["MessageBroker:RabbitMQ:Host"]!)/*,builder.Configuration["MessageBroker:RabbitMQ:VirtualHost"]*/, hostCfg =>
        {
            hostCfg.Username(builder.Configuration["MessageBroker:RabbitMQ:Username"]!);
            hostCfg.Password(builder.Configuration["MessageBroker:RabbitMQ:Password"]!);
        });
        busCfg.ConfigureEndpoints(context);
    });
    cfg.AddConsumers(Assembly.GetExecutingAssembly());
});

var app = builder.Build();

app.Run();