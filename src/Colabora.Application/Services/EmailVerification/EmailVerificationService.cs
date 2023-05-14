using Colabora.Application.Services.EmailVerification.Messages;
using Colabora.Infrastructure.Messaging;
using Colabora.Infrastructure.Messaging.Producer;
using Microsoft.Extensions.Logging;

namespace Colabora.Application.Services.EmailVerification;

public class EmailVerificationService : IEmailVerificationService
{
    private readonly ILogger<EmailVerificationService> _logger;
    private readonly IMessageProducer _messageProducer;

    public EmailVerificationService(ILogger<EmailVerificationService> logger, IMessageProducer messageProducer)
    {
        _logger = logger;
        _messageProducer = messageProducer;
    }

    public async Task SendEmailVerificationRequest(string email, CancellationToken cancellationToken)
    {
        try
        {
            if(string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Parameter 'email' is not valid.");
            
            await _messageProducer.Produce(Queue.EmailVerificationRequest ,new EmailVerificationRequest
            {
                Email = email
            }, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An exception was throw at {ValidateEmailService}", nameof(EmailVerificationService));
        }
    }
}