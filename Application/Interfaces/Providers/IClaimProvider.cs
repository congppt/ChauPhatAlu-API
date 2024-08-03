namespace Application.Interfaces.Providers;

public interface IClaimProvider
{
    T GetClaim<T>(string claimKey, T defaultValue);
}