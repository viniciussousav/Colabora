using System.Text.Json;
using Amazon.SQS;
using Colabora.Application.Services.EmailVerification.Messages;
using Colabora.Application.Shared;
using Colabora.Infrastructure.Messaging;
using Colabora.Infrastructure.Messaging.Shared;

namespace Colabora.Workers.Consumers;

// ReSharper disable once ClassNeverInstantiated.Global
public class EmailVerificationRequestConsumer : BackgroundService
{
    private readonly ILogger<EmailVerificationRequestConsumer> _logger;
    private readonly IAmazonSQS _sqs;

    public EmailVerificationRequestConsumer(ILogger<EmailVerificationRequestConsumer> logger, IAmazonSQS sqs)
    {
        _logger = logger;
        _sqs = sqs;
    }

    private static string ReceivedMessageQueue => Queue.EmailVerificationRequest;
    
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var urlResponse = await _sqs.GetQueueUrlAsync(ReceivedMessageQueue, cancellationToken);
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var response = await _sqs.ReceiveMessageAsync(ReceiveMessageRequestBuilder.Build(urlResponse.QueueUrl), cancellationToken);

                foreach (var message in response.Messages)
                {
                    var request = JsonSerializer.Deserialize<EmailVerificationRequest>(message.Body, Defaults.JsonSerializerOptions);
                    _logger.LogInformation("Email verification request received for email {Email}", request?.Email);
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