using Application.Interfaces.Databases;
using Application.Interfaces.Providers;
using Application.Interfaces.Services;
using ChauPhatAluminium.Common;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Implements.Services;

public class GenericService<T> : IGenericService<T> where T : BaseEntity
{
    public GenericService(IAppDbContext context, ITimeProvider timeProvider, IClaimProvider claimProvider,
        IPublishEndpoint publishProducer, IConfiguration config)
    {
        this.context = context;
        this.timeProvider = timeProvider;
        this.claimProvider = claimProvider;
        this.publishProducer = publishProducer;
        this.config = config;
    }

    protected readonly IAppDbContext context;
    protected readonly ITimeProvider timeProvider;
    protected readonly IClaimProvider claimProvider;
    protected readonly IPublishEndpoint publishProducer;
    protected readonly IConfiguration config;
}