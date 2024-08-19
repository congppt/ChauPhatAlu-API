using Application.Interfaces.Databases;
using Application.Interfaces.Services;
using Application.Models.Customer;
using ChauPhatAluminium.Entities;
using Mapster;
using MassTransit;

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
}