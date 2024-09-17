using Application.Models.Authorization;

namespace Application.Interfaces.Services;

public interface IAuthorizationService
{
    AuthResult Authorize(AuthRequest model);
    AuthResult Reauthorize(string refreshToken);
}