using Application.Interfaces.Providers;

namespace API.Implements.Providers;

public class ClaimProvider : IClaimProvider
{
    private readonly IHttpContextAccessor _accessor;

    public ClaimProvider(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public T GetClaim<T>(string claimType, T defaultValue)
    {
        try
        {
            var claim = _accessor.HttpContext?.User.FindFirst(claimType);
            if (claim == null) return defaultValue;
            var type = typeof(T);
            if (type.IsEnum) return (T)Enum.Parse(type, claim.Value);
            return (T)Convert.ChangeType(claim.Value, type);
        }
        catch
        {
            return defaultValue;
        }

    }
}