using RabbitEvents.Shared.Models.Messaging;
using RabbitMQ.Client;
using System.Text;

namespace RabbitEvents.Infrastructure.Messaging;

public sealed class QueueService : IQueueService, IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public QueueService()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };

        _connection = factory.CreateConnection();

        _channel = _connection.CreateModel();
    }

    private void ExchangeDeclare(string exchange, string exchangeType) =>
        _channel.ExchangeDeclare(exchange: exchange,
                                 type: exchangeType,
                                 durable: true,
                                 autoDelete: false,
                                 arguments: null);

    private void QueueDeclare(string queue) =>
        _channel.QueueDeclare(queue: queue,
                              durable: true,
                              exclusive: false,
                              autoDelete: false,
                              arguments: null);

    private void QueueBind(string queue, string exchange, string routingKey) =>
        _channel.QueueBind(queue: queue,
                           exchange: exchange,
                           routingKey: routingKey);

    public void CreateMessageChannel(Queue queue, Exchange exchange)
    {
        try
        {
            ExchangeDeclare(exchange.Name, exchange.Type);

            QueueDeclare(queue.Name);

            if (string.IsNullOrEmpty(exchange.Name) is false && string.IsNullOrEmpty(queue.RoutingKey) is false)
                QueueBind(queue.Name, exchange.Name, queue.RoutingKey);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void SendMessage(QueueMessage message)
    {
        CreateMessageChannel(message.Queue, message.Exchange);

        var messageBodyBytes = Encoding.UTF8.GetBytes(message.MessageBody);

        IBasicProperties props = _channel.CreateBasicProperties();

        props.DeliveryMode = 1; //non-persistent

        _channel.BasicPublish(message.Exchange.Name, message.RoutingKey, props, messageBodyBytes);
    }

    public void Dispose()
    {
        _channel.Close();
        _connection.Close();
    }
}
