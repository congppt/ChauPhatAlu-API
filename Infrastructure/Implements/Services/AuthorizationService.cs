using Application.Interfaces.Services;
using Application.Models.Authorization;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Implements.Services;

public class AuthorizationService : IAuthorizationService
{
    private readonly IConfiguration _config;

    public AuthorizationService(IConfiguration config)
    {
        _config = config;
    }

    public AuthResult Authorize(AuthRequest model)
    {
        throw new NotImplementedException();
    }

    public AuthResult Reauthorize(string refreshToken)
    {
        throw new NotImplementedException();
    }
}