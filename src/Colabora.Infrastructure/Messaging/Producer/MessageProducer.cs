using Amazon.SQS;
using Colabora.Infrastructure.Messaging.Configuration;

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
        
        var urlResponse = await _sqs.GetQueueUrlAsync(queueUrl, cancellationToken);
        await _sqs.SendMessageAsync(SendMessageRequestBuilder.Build(urlResponse.QueueUrl, message), cancellationToken);
    }
}