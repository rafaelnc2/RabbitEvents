using RabbitEvents.Shared.Models.Messaging;

namespace RabbitEvents.Application.Interfaces;

public interface IQueueService
{
    void CreateTopic(Queue queue, Exchange exchange);

    void CreateQueue(Queue queue);

    void SendMessage(QueueMessage message);
}
