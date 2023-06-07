namespace Colabora.Infrastructure.Auth.Google;

public class GoogleAuthSettings
{
    public const string Key = "AuthenticationSettings::GoogleAuthSettings";
    
    public string AudienceAndroid { get; init; } = string.Empty;
    
    public string AudienceIos { get; init; } = string.Empty;
    
    public string AudienceWeb { get; init; } = string.Empty;
    public long ExpirationTime { get; init; } = TimeSpan.FromHours(1).Milliseconds;
}