using Application.Interfaces.Databases;
using Application.Interfaces.Providers;
using Application.Interfaces.Services;
using ChauPhatAluminium.Common;
using MassTransit;

namespace Infrastructure.Implements.Services;

public class GenericService<T> : IGenericService<T> where T : BaseEntity
{
    public GenericService(IAppDbContext context, ITimeProvider timeProvider, IClaimProvider claimProvider)
    {
        this.context = context;
        this.timeProvider = timeProvider;
        this.claimProvider = claimProvider;
    }

    protected readonly IAppDbContext context;
    protected readonly ITimeProvider timeProvider;
    protected readonly IClaimProvider claimProvider;
    protected readonly IPublishEndpoint producer;
}