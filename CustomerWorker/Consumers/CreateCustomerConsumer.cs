using System.Net;
using Application.Interfaces.Databases;
using Application.Models.Common;
using Application.Models.Customer;
using ChauPhatAluminium.Entities;
using FluentValidation;
using Mapster;
using MassTransit;
using RabbitMQ.Client;

namespace CustomerWorker.Consumers;

public class CreateCustomerConsumer : IConsumer<CreateCustomer>
{
    private readonly IAppDbContext _dbContext;
    private readonly IValidator<CreateCustomer> _validator;
    public CreateCustomerConsumer(IAppDbContext dbContext, IValidator<CreateCustomer> validator)
    {
        _dbContext = dbContext;
        _validator = validator;
    }

    public async Task Consume(ConsumeContext<CreateCustomer> context)
    {
        var validation = await _validator.ValidateAsync(context.Message);
        var response = new MessageResult
        {
            StatusCode = HttpStatusCode.Conflict,
            Data = validation
        };
        if (validation.IsValid)
        {
            try
            {
                var customer = context.Message.Adapt<Customer>();
                await _dbContext.Customers.AddAsync(customer);
                await _dbContext.SaveChangesAsync();
                response.StatusCode = HttpStatusCode.Created;
                response.Data = customer.Adapt<DetailCustomerInfo>();
            }
            catch (Exception ex)
            {
                
            }
        }
    }
    
}
public class CreateCustomerConsumerDefinition : ConsumerDefinition<CreateCustomerConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<CreateCustomerConsumer> consumerConfigurator)
    {
        endpointConfigurator.ConfigureConsumeTopology = false;
    
        if (endpointConfigurator is IRabbitMqReceiveEndpointConfigurator rmq)
        {
            rmq.Bind<CreateCustomer>(x =>
            {
                x.ExchangeType = ExchangeType.Direct;
            });
        }
    }
}