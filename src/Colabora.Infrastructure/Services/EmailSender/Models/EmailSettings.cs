namespace Colabora.Infrastructure.Services.EmailSender.Models;

public class EmailSettings
{
    public const string ConfigKey = "EmailSettings::Google";

    public string SmtpHost { get; set; } = string.Empty;

    public int SmtpPort { get; set; } = 0;
    
    public string SmtpUser { get; set; } = string.Empty;
    
    public string SmtpPass { get; set; } = string.Empty;
}