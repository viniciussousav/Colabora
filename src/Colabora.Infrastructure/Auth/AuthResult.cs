using System.Text.Json.Serialization;

namespace Colabora.Infrastructure.Auth;

public class AuthResult
{
    public string Token { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    [JsonIgnore] public string Error { get; init; } = string.Empty;
    [JsonIgnore] public bool IsValid => !string.IsNullOrWhiteSpace(Token);
}