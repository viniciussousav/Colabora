using Colabora.Infrastructure.Services.EmailSender.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace Colabora.Infrastructure.Services.EmailSender;

public class EmailSender : IEmailSender
{
    private readonly SmtpClient _smtpClient;
    private readonly EmailSettings _appSettings;

    public EmailSender(IOptions<EmailSettings> appSettings)
    {
        _appSettings = appSettings.Value;
        _smtpClient = new SmtpClient();
    }

    public async Task SendEmail(string to, string subject, string template)
    {
        var email = new MimeMessage
        {
            From = {MailboxAddress.Parse(_appSettings.ServiceAccount)},
            To = {MailboxAddress.Parse(to)},
            Subject = subject,
            Body = new TextPart(TextFormat.Html) {Text = template}
        };

        await _smtpClient.ConnectAsync(_appSettings.SmtpHost, _appSettings.SmtpPort, SecureSocketOptions.StartTls);
        await _smtpClient.AuthenticateAsync(_appSettings.SmtpUser, _appSettings.SmtpPass);
        await _smtpClient.SendAsync(email);
        await _smtpClient.DisconnectAsync(true);
    }
}