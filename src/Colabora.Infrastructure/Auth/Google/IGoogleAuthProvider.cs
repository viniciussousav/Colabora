using Colabora.Infrastructure.Auth.Shared;

namespace Colabora.Infrastructure.Auth.Google;

public interface IGoogleAuthProvider
{
    Task<UserAuthInfo> Authenticate(string token);
}