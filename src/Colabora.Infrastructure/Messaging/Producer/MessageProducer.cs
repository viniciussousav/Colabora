using Amazon.SQS;
using Colabora.Infrastructure.Messaging.Shared;

namespace Colabora.Infrastructure.Messaging.Producer;

public class MessageProducer : IMessageProducer
{
    private readonly IAmazonSQS _sqs;

    public MessageProducer(IAmazonSQS sqs)
    {
        _sqs = sqs;
    }

    public async Task Produce<T>(string queueUrl, T message, CancellationToken cancellationToken)
    {
        if (message is null)
            throw new ArgumentNullException(nameof(message));

        await _sqs.SendMessageAsync(SendMessageRequestBuilder.Build(queueUrl, message), cancellationToken);
    }
}