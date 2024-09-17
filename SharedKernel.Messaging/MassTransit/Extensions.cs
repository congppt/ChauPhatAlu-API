using System.Reflection;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SharedKernel.Messaging.MassTransit;

public static class Extensions
{
    public static IServiceCollection AddMessageBroker(this IServiceCollection services, IConfiguration config,
        Assembly? assembly = null)
    {
        services.AddMassTransit(cfg =>
        {
            cfg.SetKebabCaseEndpointNameFormatter();
            if (assembly != null) cfg.AddConsumers(assembly);
            cfg.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(config["MessageBroker:Host"],config["MessageBroker:VirtualHost"], hostConfigurator =>
                {
                    hostConfigurator.Username(config["MessageBroker:RabbitMQ:Username"]!);
                    hostConfigurator.Password(config["MessageBroker:RabbitMQ:Password"]!);
                });
                configurator.ConfigureEndpoints(context);
            });
        });
        return services;
    }
}