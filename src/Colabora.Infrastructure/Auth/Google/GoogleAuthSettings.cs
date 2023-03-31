namespace Colabora.Infrastructure.Auth.Google;

public class GoogleAuthSettings
{
    public const string Key = "AuthenticationSettings::GoogleAuthSettings";
    public string AudienceAndroid { get; init; }
    public string AudienceIOS { get; init; }
    public string AudienceWeb { get; init; }
    public TimeSpan ExpirationTime { get; init; }
}