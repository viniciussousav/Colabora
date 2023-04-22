using Colabora.Infrastructure.Auth.Shared;

namespace Colabora.Infrastructure.Auth.Google;

public interface IGoogleAuthService
{
    Task<UserAuthInfo> Authenticate(string token);
}