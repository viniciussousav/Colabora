using System.Text.Json;
using Amazon.SQS;
using Colabora.Application.Features.Organization.RegisterOrganization.Models;
using Colabora.Application.Mappers;
using Colabora.Application.Services.EmailVerification;
using Colabora.Application.Shared;
using Colabora.Infrastructure.Messaging;
using Colabora.Infrastructure.Messaging.Configuration;
using Colabora.Infrastructure.Persistence.DynamoDb.Repositories.EmailVerification.Models;

namespace Colabora.Workers.Consumers;

public class EmailVerificationRequestConsumer : BackgroundService
{
    private readonly ILogger<EmailVerificationRequestConsumer> _logger;
    private readonly IAmazonSQS _sqs;
    private readonly IEmailVerificationService _emailVerificationService;
    
    public EmailVerificationRequestConsumer(
        ILogger<EmailVerificationRequestConsumer> logger,
        IAmazonSQS sqs, 
        IEmailVerificationService emailVerificationService)
    {
        _logger = logger;
        _sqs = sqs;
        _emailVerificationService = emailVerificationService;
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
                    var organization = JsonSerializer.Deserialize<RegisterOrganizationResponse>(message.Body, Defaults.JsonSerializerOptions)
                        ?? throw new JsonException("Received message is not a valid JSON object");
                    
                    _logger.LogInformation("Organization registered event received for email {Email}", organization.Email);

                    var result = await _emailVerificationService.SendEmailVerification(new EmailVerificationRequest(organization.Email));

                    if (!result.IsValid)
                    {
                        _logger.LogWarning("Error while sending email verification to {Email}, skipping delete.", organization.Email);
                        continue;   
                    }
                    
                    _logger.LogInformation("Email verification sent to {Email}", organization.Email);
                    await _sqs.DeleteMessageAsync(urlResponse.QueueUrl, message.ReceiptHandle, cancellationToken);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An exception occurred at {EmailVerificationRequestConsumer}", nameof(EmailVerificationRequestConsumer));
            }
        }
    }
}