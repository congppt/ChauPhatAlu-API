using API.Implements.Providers;
using Application.Interfaces.Providers;

namespace API;

public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddHttpContextAccessor();
        services.AddSingleton<IClaimProvider, ClaimProvider>();
        return services;
    }
}