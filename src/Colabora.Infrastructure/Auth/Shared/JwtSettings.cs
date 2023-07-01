namespace Colabora.Infrastructure.Auth.Shared;

public class JwtSettings
{
    public const string Key = "AuthenticationSettings";
    
    public string JwtKey { get; init; } = string.Empty;
    public string JwtIssuer { get; init; } = string.Empty;
    public string JwtAudience { get; init; } = string.Empty;
    public TimeSpan ExpirationTime { get; init; } = TimeSpan.FromHours(72);
}