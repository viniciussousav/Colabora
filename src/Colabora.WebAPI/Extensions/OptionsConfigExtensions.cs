using Colabora.Infrastructure.Auth.Google;
using Colabora.Infrastructure.Auth.Shared;

namespace Colabora.WebAPI.Extensions;

public static class OptionsConfigExtensions
{
    public static void AddOptionsConfig(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddOptions<JwtSettings>().BindConfiguration(JwtSettings.Key);
        serviceCollection.AddOptions<GoogleAuthSettings>().BindConfiguration(GoogleAuthSettings.Key);
    }
}