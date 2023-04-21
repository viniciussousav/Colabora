namespace Colabora.Infrastructure.Auth;

public class AuthResult
{
    public string Token { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Error { get; init; } = string.Empty;
    public bool IsValid => !string.IsNullOrWhiteSpace(Token);
}