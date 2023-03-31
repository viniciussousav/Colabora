using Colabora.Infrastructure.Auth.Shared;
using Google.Apis.Auth;
using Microsoft.Extensions.Options;

namespace Colabora.Infrastructure.Auth.Google;

public class GoogleAuthService : IGoogleAuthService
{
    private readonly GoogleJsonWebSignature.ValidationSettings _validationSettings;

    public GoogleAuthService(IOptions<GoogleAuthSettings> googleAuthSettingsOptions)
    {
        var googleAuthSettings = googleAuthSettingsOptions.Value;
        _validationSettings = new GoogleJsonWebSignature.ValidationSettings
        {
            Audience = new []
            {   
                googleAuthSettings.AudienceAndroid,
                googleAuthSettings.AudienceIOS,
                googleAuthSettings.AudienceWeb
            },
            ExpirationTimeClockTolerance = googleAuthSettings.ExpirationTime
        };
    }
    
    public async Task<UserAuthInfo> Authenticate(string token)
    {
        var payload = await GoogleJsonWebSignature.ValidateAsync(token, _validationSettings);
        return new UserAuthInfo(email: payload.Email, name: payload.Name);
    }
}

