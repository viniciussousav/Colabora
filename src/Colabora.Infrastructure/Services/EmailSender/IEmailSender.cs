namespace Colabora.Infrastructure.Services.EmailSender;

public interface IEmailSender
{
    Task SendEmail(string to, string subject, string template);
}