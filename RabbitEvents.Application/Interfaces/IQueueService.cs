using RabbitEvents.Shared.Models.Messaging;

namespace RabbitEvents.Application.Interfaces;

public interface IQueueService
{
    void CreateTopic(Queue queue, Exchange exchange);

    void CreateQueue(Queue queue);

    void CreateDeadLetterQueue(Queue queue, Exchange exchange);

    void SendMessage(QueueMessage message);

    Task ConsumeQueue(string queueName, Func<string, ulong, Task> messageHandlerAsync, CancellationToken cancellationToken);
}
