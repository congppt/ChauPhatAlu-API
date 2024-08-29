using Application.Interfaces.Databases;
using Application.Interfaces.Providers;
using Application.Interfaces.Services;
using Application.Models.Common;
using Application.Models.Customer;
using ChauPhatAluminium.Entities;
using Mapster;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implements.Services;

public class CustomerService : GenericService<Customer>, ICustomerService
{
    public CustomerService(IAppDbContext context, ITimeProvider timeProvider, IClaimProvider claimProvider,
        IPublishEndpoint publishProducer) : base(context, timeProvider, claimProvider, publishProducer)
    {
    }

    public async Task<OffsetPage<BasicCustomerInfo>> GetCustomerPageAsync(int pageNumber, int pageSize, string? phone,
        string? name, CancellationToken ct = default)
    {
        var source = context.GetUntrackedQuery<Customer>();
        if (!string.IsNullOrWhiteSpace(phone))
            source = source.Where(c => c.Phone.Contains(phone));
        if (!string.IsNullOrWhiteSpace(name))
            source = source.Where(c => EF.Functions.ILike(c.Name, $"%{name}%"));
        source = source.OrderByDescending(c => c.Id);
        return await OffsetPage<BasicCustomerInfo>.CreateAsync(source.ProjectToType<BasicCustomerInfo>(), pageNumber,
            pageSize, ct);
    }

    public async Task<DetailCustomerInfo> GetCustomerAsync(int customerId, CancellationToken ct = default)
    {
        var customer = await context.GetByIdAsync<Customer>(customerId, ct) ?? throw new KeyNotFoundException();
        return customer.Adapt<DetailCustomerInfo>();
    }

    public async Task<Guid> CreateCustomerAsync(CustomerCreate model)
    {
        await publishProducer.Publish(model);
        return model.Guid;
    }
}