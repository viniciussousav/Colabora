using Colabora.Infrastructure.Auth.Shared;
using Google.Apis.Auth;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Colabora.Infrastructure.Auth.Google;

public class GoogleAuthProvider : IGoogleAuthProvider
{
    private readonly ILogger<GoogleAuthProvider> _logger;
    private readonly GoogleJsonWebSignature.ValidationSettings _validationSettings;

    public GoogleAuthProvider(ILogger<GoogleAuthProvider> logger, IOptions<GoogleAuthSettings> googleAuthSettingsOptions)
    {
        _logger = logger;
        
        var googleAuthSettings = googleAuthSettingsOptions.Value;
        _validationSettings = new GoogleJsonWebSignature.ValidationSettings
        {
            ExpirationTimeClockTolerance = TimeSpan.FromHours(googleAuthSettings.ExpirationTime)
        };
    }
    
    public async Task<UserAuthInfo> Authenticate(string token)
    {
        var payload = await GoogleJsonWebSignature.ValidateAsync(token, _validationSettings);
        return new UserAuthInfo(email: payload.Email, name: payload.Name);
    }
}

