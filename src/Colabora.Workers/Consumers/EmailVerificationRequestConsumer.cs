using System.Net.Mail;
using System.Text.Json;
using Amazon.SQS;
using Colabora.Application.Features.Organization.RegisterOrganization.Models;
using Colabora.Application.Shared;
using Colabora.Infrastructure.Messaging;
using Colabora.Infrastructure.Messaging.Configuration;
using Colabora.Infrastructure.Persistence.DynamoDb.Repositories.EmailVerification;
using Colabora.Infrastructure.Persistence.DynamoDb.Repositories.EmailVerification.Models;
using Colabora.Infrastructure.Services.EmailSender;

namespace Colabora.Workers.Consumers;

// ReSharper disable once ClassNeverInstantiated.Global
public class EmailVerificationRequestConsumer : BackgroundService
{
    private readonly ILogger<EmailVerificationRequestConsumer> _logger;
    private readonly IAmazonSQS _sqs;
    private readonly IEmailSender _emailSender;
    private readonly IEmailVerificationRepository _emailVerificationRepository;

    public EmailVerificationRequestConsumer(
        ILogger<EmailVerificationRequestConsumer> logger,
        IAmazonSQS sqs,
        IEmailSender emailSender,
        IEmailVerificationRepository emailVerificationRepository)
    {
        _logger = logger;
        _sqs = sqs;
        _emailSender = emailSender;
        _emailVerificationRepository = emailVerificationRepository;
    }

    private const string ConsumedQueue = Queues.OrganizationRegistered;

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var urlResponse = await _sqs.GetQueueUrlAsync(ConsumedQueue, cancellationToken);

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var response = await _sqs.ReceiveMessageAsync(ReceiveMessageRequestBuilder.Build(urlResponse.QueueUrl), cancellationToken);

                foreach (var message in response.Messages)
                {
                    var request = JsonSerializer.Deserialize<RegisterOrganizationResponse>(message.Body, Defaults.JsonSerializerOptions);
                    _logger.LogInformation("Organization registered event received for email {Email}", request?.Email);

                    if (!IsValidEmail(request?.Email))
                    {
                        var emailVerificationRequest = new EmailVerificationRequest
                        {
                            Code = Guid.NewGuid(),
                            Email = request!.Email,
                            ExpirationTime = DateTimeOffset.UtcNow.AddHours(2)
                        };

                        await _emailVerificationRepository.CreateEmailVerificationRequest(emailVerificationRequest);
                        await _emailSender.SendEmail(request.Email, "Valide seu email", "Test");
                    }

                    await _sqs.DeleteMessageAsync(urlResponse.QueueUrl, message.ReceiptHandle, cancellationToken);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An exception occurred at {EmailVerificationRequestConsumer}", nameof(EmailVerificationRequestConsumer));
                throw;
            }
        }
    }

    private static bool IsValidEmail(string? email) =>
        !string.IsNullOrWhiteSpace(email) || !MailAddress.TryCreate(email!, out _);
}