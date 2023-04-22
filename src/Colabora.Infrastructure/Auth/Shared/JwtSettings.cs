namespace Colabora.Infrastructure.Auth.Shared;

public class JwtSettings
{
    public const string Key = "AuthenticationSettings";
    
    public string JwtKey { get; init; }
    public string JwtIssuer { get; init; }
    public string JwtAudience { get; init; }
    public TimeSpan ExpirationTime { get; init; }
}