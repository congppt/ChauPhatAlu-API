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

public class UpdateCustomerConsumer : IConsumer<UpdateCustomer>
{
    private readonly IAppDbContext _dbContext;
    private readonly IValidator<UpdateCustomer> _validator;
    public UpdateCustomerConsumer(IAppDbContext dbContext, IValidator<UpdateCustomer> validator)
    {
        _dbContext = dbContext;
        _validator = validator;
    }

    public async Task Consume(ConsumeContext<UpdateCustomer> context)
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
                response.StatusCode = HttpStatusCode.OK;
                response.Data = customer.Adapt<DetailCustomerInfo>();
            }
            catch (Exception ex)
            {
                
            }
        }
    }
    
}
public class UpdateCustomerConsumerDefinition : ConsumerDefinition<UpdateCustomerConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<UpdateCustomerConsumer> consumerConfigurator)
    {
        endpointConfigurator.ConfigureConsumeTopology = false;
    
        if (endpointConfigurator is IRabbitMqReceiveEndpointConfigurator rmq)
        {
            rmq.Bind<UpdateCustomer>(x =>
            {
                x.ExchangeType = ExchangeType.Direct;
            });
        }
    }
}