using System.Net;
using Application.Interfaces.Databases;
using Application.Interfaces.Providers;
using Application.Models.Common;
using Application.Models.Order;
using ChauPhatAluminium.Entities;
using FluentValidation;
using Mapster;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;

namespace OrderWorker.Consumers;

public class UpdateOrderStatusConsumer : IConsumer<UpdateOrderStatus>
{
    private readonly IAppDbContext _dbContext;
    private readonly IValidator<UpdateOrderStatus> _validator;
    private readonly ITimeProvider _timeProvider;
    public UpdateOrderStatusConsumer(IAppDbContext dbContext, IValidator<UpdateOrderStatus> validator, ITimeProvider timeProvider)
    {
        _dbContext = dbContext;
        _validator = validator;
        _timeProvider = timeProvider;
    }
    public async Task Consume(ConsumeContext<UpdateOrderStatus> context)
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
                var order = await _dbContext.GetByIdAsync<Order>(context.Message.Id, default,
                    o => o.Details.AsQueryable().Include(d => d.Product)) ?? throw new KeyNotFoundException();
                order.Status++;
                order.Traces.Add(new() { Status = order.Status, ModifiedAt = _timeProvider.Now(), Note = context.Message.Note });
                await _dbContext.SaveChangesAsync();
                response.StatusCode = HttpStatusCode.OK;
                response.Data = order.Adapt<DetailOrderInfo>();
            }
            catch (Exception ex)
            {
                
            }
            
        }
        
    }
}
public class UpdateOrderStatusConsumerDefinition : ConsumerDefinition<UpdateOrderStatusConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<UpdateOrderStatusConsumer> consumerConfigurator)
    {
        endpointConfigurator.ConfigureConsumeTopology = false;
    
        if (endpointConfigurator is IRabbitMqReceiveEndpointConfigurator rmq)
        {
            rmq.Bind<CreateOrder>(x =>
            {
                x.ExchangeType = ExchangeType.Direct;
            });
        }
    }
}