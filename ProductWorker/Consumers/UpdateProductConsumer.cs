using System.Net;
using Application.Interfaces.Databases;
using Application.Models.Common;
using Application.Models.Product;
using ChauPhatAluminium.Entities;
using FluentValidation;
using Mapster;
using MassTransit;
using RabbitMQ.Client;

namespace ProductWorker.Consumers;

public class UpdateProductConsumer : IConsumer<UpdateProduct>
{
    private readonly IAppDbContext _dbContext;
    private readonly IValidator<UpdateProduct> _validator;
    public UpdateProductConsumer(IAppDbContext dbContext, IValidator<UpdateProduct> validator)
    {
        _dbContext = dbContext;
        _validator = validator;
    }

    public async Task Consume(ConsumeContext<UpdateProduct> context)
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
                var customer = context.Message.Adapt<Product>();
                await _dbContext.Products.AddAsync(customer);
                await _dbContext.SaveChangesAsync();
                response.StatusCode = HttpStatusCode.OK;
                response.Data = customer.Adapt<DetailProductInfo>();
            }
            catch (Exception ex)
            {
                
            }
        }
    }
    
}
public class UpdateProductConsumerDefinition : ConsumerDefinition<UpdateProductConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<UpdateProductConsumer> consumerConfigurator)
    {
        endpointConfigurator.ConfigureConsumeTopology = false;
    
        if (endpointConfigurator is IRabbitMqReceiveEndpointConfigurator rmq)
        {
            rmq.Bind<UpdateProduct>(x =>
            {
                x.ExchangeType = ExchangeType.Direct;
            });
        }
    }
}