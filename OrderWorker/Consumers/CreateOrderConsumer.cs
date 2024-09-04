using System.Net;
using Application.Interfaces.Databases;
using Application.Interfaces.Providers;
using Application.Models.Common;
using Application.Models.Order;
using ChauPhatAluminium.Entities;
using ChauPhatAluminium.Enums;
using FluentValidation;
using Mapster;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;

namespace OrderWorker.Consumers;

public class CreateOrderConsumer : IConsumer<CreateOrder>
{
    private readonly IAppDbContext _dbContext;
    private readonly ITimeProvider _timeProvider;
    private readonly IValidator<CreateOrder> _validator;
    public CreateOrderConsumer(IAppDbContext dbContext, IValidator<CreateOrder> validator, ITimeProvider timeProvider)
    {
        _dbContext = dbContext;
        _validator = validator;
        _timeProvider = timeProvider;
    }

    public async Task Consume(ConsumeContext<CreateOrder> context)
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
                var model = context.Message;
                var order = model.Adapt<Order>();
                order.Details = [];
                var total = 0m;
                var products = await _dbContext.Products.Where(p => model.Products.Keys.Contains(p.Id)).ToListAsync();
                foreach (var product in products)
                {
                    var detail = model.Products[product.Id].Adapt<OrderDetail>();
                    detail.Product = product;
                    detail.Price = product.Price;
                    detail.Total = detail.Price * detail.Quantity;
                    switch (product.Category)
                    {
                        case Category.AluminiumDoor:
                        case Category.RollingDoor:
                        case Category.SlidingDoor:
                            detail.Total *= detail.Height * detail.Width;
                            break;
                    }
                    total += detail.Total;
                    order.Details.Add(detail);
                }
                order.Customer = await _dbContext.GetByIdAsync<Customer>(order.CustomerId);
                order.Total = total;
                order.Status = OrderStatus.Processing;
                order.CreatedAt = _timeProvider.Now();
                order.Traces = [ new Order.Trace
                {
                    ModifiedAt = order.CreatedAt,
                    Note = "",
                    Status = order.Status
                }];
                await _dbContext.Orders.AddAsync(order);
                await _dbContext.SaveChangesAsync();
                response.StatusCode = HttpStatusCode.Created;
                response.Data = order.Adapt<DetailOrderInfo>();
            }
            catch (Exception ex)
            {
                
            }
        }
    }
    
}
public class CreateOrderConsumerDefinition : ConsumerDefinition<CreateOrderConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<CreateOrderConsumer> consumerConfigurator)
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