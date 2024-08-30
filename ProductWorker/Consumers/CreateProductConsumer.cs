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

public class CreateProductConsumer : IConsumer<CreateProduct>
{
    private readonly IAppDbContext _dbContext;
    private readonly IValidator<CreateProduct> _validator;
    public CreateProductConsumer(IAppDbContext dbContext, IValidator<CreateProduct> validator)
    {
        _dbContext = dbContext;
        _validator = validator;
    }

    public async Task Consume(ConsumeContext<CreateProduct> context)
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
                response.StatusCode = HttpStatusCode.Created;
                response.Data = customer.Adapt<DetailProductInfo>();
            }
            catch (Exception ex)
            {
                
            }
        }
    }
    
}
public class CreateProductConsumerDefinition : ConsumerDefinition<CreateProductConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<CreateProductConsumer> consumerConfigurator)
    {
        endpointConfigurator.ConfigureConsumeTopology = false;
    
        if (endpointConfigurator is IRabbitMqReceiveEndpointConfigurator rmq)
        {
            rmq.Bind<CreateProduct>(x =>
            {
                x.ExchangeType = ExchangeType.Direct;
            });
        }
    }
}