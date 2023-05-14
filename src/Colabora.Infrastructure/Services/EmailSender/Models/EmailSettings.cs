namespace Colabora.Infrastructure.Services.EmailSender.Models;

public class EmailSettings
{
    public const string Key = "Smtp:Email";
    
    public string ServiceAccount { get; init; }
    public string SmtpHost { get; init; }
    public int SmtpPort { get; init; }
    public string SmtpUser { get; init; }
    public string SmtpPass { get; init; }
}