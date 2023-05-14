using Colabora.Infrastructure.Auth.Google;
using Colabora.Infrastructure.Auth.Shared;
using Colabora.Infrastructure.Services.EmailSender.Models;

namespace Colabora.WebAPI.Extensions;

public static class OptionsConfigExtensions
{
    public static void AddOptionsConfig(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddOptions<JwtSettings>().BindConfiguration(JwtSettings.Key);
        serviceCollection.AddOptions<GoogleAuthSettings>().BindConfiguration(GoogleAuthSettings.Key);
        serviceCollection.AddOptions<EmailSettings>().BindConfiguration(EmailSettings.Key);
    }
}