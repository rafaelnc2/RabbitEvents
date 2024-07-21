using RabbitEvents.Shared.Models.Messaging;

namespace RabbitEvents.Domain.Interfaces.Messaging;
public interface IRabbitMQService
{
    void CreateMessageChannel(string queue, Exchange exchange, string routingKey);
    void SendMessage(string queueName, Exchange exchange, string routingKey, string message);
}
