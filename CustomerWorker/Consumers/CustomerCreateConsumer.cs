using Application.Interfaces.Databases;
using Application.Models.Customer;
using ChauPhatAluminium.Entities;
using Mapster;
using MassTransit;
using RabbitMQ.Client;

namespace ConsumerWorker.Consumers;

public class CustomerCreateConsumer : IConsumer<CustomerCreate>
{
    private readonly IAppDbContext _dbContext;
    public CustomerCreateConsumer(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<CustomerCreate> context)
    {
        var customer = context.Message.Adapt<Customer>();
        await _dbContext.Customers.AddAsync(customer);
        await _dbContext.SaveChangesAsync();
    }
    public class CustomerCreateConsumerDefinition :
        ConsumerDefinition<CustomerCreateConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
            IConsumerConfigurator<CustomerCreateConsumer> consumerConfigurator)
        {
            endpointConfigurator.ConfigureConsumeTopology = false;
    
            if (endpointConfigurator is IRabbitMqReceiveEndpointConfigurator rmq)
            {
                rmq.Bind<CustomerCreate>(x =>
                {
                    x.ExchangeType = ExchangeType.Direct;
                });
            }
        }
    }
}