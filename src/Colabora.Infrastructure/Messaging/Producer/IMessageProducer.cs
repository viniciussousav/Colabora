namespace Colabora.Infrastructure.Messaging.Producer;

public interface IMessageProducer
{
    Task Produce<T>(string queueUrl,T message, CancellationToken cancellationToken);
}