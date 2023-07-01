using Colabora.Infrastructure.Services.EmailSender.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace Colabora.Infrastructure.Services.EmailSender;

public class EmailSender : IEmailSender
{
    private readonly EmailSettings _appSettings;

    public EmailSender(IOptions<EmailSettings> appSettings)
    {
        _appSettings = appSettings.Value;
    }

    public async Task SendEmail(string to, string subject, string template)
    {
        var email = new MimeMessage
        {
            From = {MailboxAddress.Parse(_appSettings.SmtpUser)},
            To = {MailboxAddress.Parse(to)},
            Subject = subject,
            Body = new TextPart(TextFormat.Html) {Text = template}
        };

        var smtpClient = new SmtpClient();
        smtpClient.ServerCertificateValidationCallback = (_, _, _, _) => true;

        await smtpClient.ConnectAsync(_appSettings.SmtpHost, _appSettings.SmtpPort, false);
        await smtpClient.AuthenticateAsync(_appSettings.SmtpUser, _appSettings.SmtpPass);
        await smtpClient.SendAsync(email);
        await smtpClient.DisconnectAsync(true);
    }
}