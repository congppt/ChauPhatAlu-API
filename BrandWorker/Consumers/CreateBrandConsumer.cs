using System.Net;
using Application.Interfaces.Databases;
using Application.Models.Brand;
using Application.Models.Common;
using ChauPhatAluminium.Entities;
using FluentValidation;
using Mapster;
using MassTransit;
using RabbitMQ.Client;

namespace BrandWorker.Consumers;

public class CreateBrandConsumer : IConsumer<CreateBrand>
{
    private readonly IAppDbContext _dbContext;
    private readonly IValidator<CreateBrand> _validator;
    public CreateBrandConsumer(IAppDbContext dbContext, IValidator<CreateBrand> validator)
    {
        _dbContext = dbContext;
        _validator = validator;
    }

    public async Task Consume(ConsumeContext<CreateBrand> context)
    {
        var validation = await _validator.ValidateAsync(context.Message);
        var response = new CommandResult
        {
            StatusCode = HttpStatusCode.Conflict,
            Data = validation
        };
        if (validation.IsValid)
        {
            try
            {
                var brand = context.Message.Adapt<Brand>();
                await _dbContext.Brands.AddAsync(brand);
                await _dbContext.SaveChangesAsync();
                response.StatusCode = HttpStatusCode.Created;
                response.Data = brand.Adapt<DetailBrandInfo>();
            }
            catch (Exception ex)
            {
                
            }
        }
    }
    
}
public class CreateBrandConsumerDefinition : ConsumerDefinition<CreateBrandConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<CreateBrandConsumer> consumerConfigurator)
    {
        endpointConfigurator.ConfigureConsumeTopology = false;
    
        if (endpointConfigurator is IRabbitMqReceiveEndpointConfigurator rmq)
        {
            rmq.Bind<CreateBrand>(x =>
            {
                x.ExchangeType = ExchangeType.Direct;
            });
        }
    }
}