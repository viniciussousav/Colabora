namespace Colabora.Infrastructure.Auth;

public interface IAuthService
{
    Task<AuthResult> AuthenticateUser(AuthProvider authProvider, string token);
}