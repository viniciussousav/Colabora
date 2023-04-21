using Colabora.Infrastructure.Auth.Shared;
using Google.Apis.Auth;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Colabora.Infrastructure.Auth.Google;

public class GoogleAuthService : IGoogleAuthService
{
    private readonly ILogger<GoogleAuthService> _logger;
    private readonly GoogleJsonWebSignature.ValidationSettings _validationSettings;

    public GoogleAuthService(ILogger<GoogleAuthService> logger, IOptions<GoogleAuthSettings> googleAuthSettingsOptions)
    {
        _logger = logger;
        
        var googleAuthSettings = googleAuthSettingsOptions.Value;
        _validationSettings = new GoogleJsonWebSignature.ValidationSettings
        {
            Audience = new []
            {   
                googleAuthSettings.AudienceAndroid,
                googleAuthSettings.AudienceIOS,
                googleAuthSettings.AudienceWeb
            },
            ExpirationTimeClockTolerance = TimeSpan.FromHours(googleAuthSettings.ExpirationTime)
        };
    }
    
    public async Task<UserAuthInfo> Authenticate(string token)
    {
        _logger.LogInformation("[GOOGLE AUTH SETTINGS]: {0}", string.Join(",", _validationSettings.Audience));
        var payload = await GoogleJsonWebSignature.ValidateAsync(token, _validationSettings);
        return new UserAuthInfo(email: payload.Email, name: payload.Name);
    }
}

