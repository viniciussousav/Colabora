using Colabora.Infrastructure.Services.EmailSender.Models;

namespace Colabora.Workers.Configuration;

public static class OptionsConfigurationExtensions
{
    public static void AddOptionsConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<EmailSettings>()
            .Bind(configuration.GetSection(EmailSettings.ConfigKey))
            .Validate(settings => !string.IsNullOrWhiteSpace(settings.SmtpHost) &&
                                  !string.IsNullOrWhiteSpace(settings.SmtpPass) &&
                                  !string.IsNullOrWhiteSpace(settings.SmtpUser) &&
                                  settings.SmtpPort != 0, 
                failureMessage: "Error on EmailSettings validation");
    }
}