using RabbitEvents.Shared.Models.Messaging;

namespace RabbitEvents.Application.Interfaces;

public interface IQueueService
{
    void CreateMessageChannel(Queue queue, Exchange exchange);
    //void CreateMessageChannel(string queue, Exchange exchange, string routingKey);
    void SendMessage(QueueMessage message);
}
