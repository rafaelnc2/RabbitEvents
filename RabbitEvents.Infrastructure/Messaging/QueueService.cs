using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitEvents.Infrastructure.Messaging;

public sealed class QueueService : IQueueService, IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    private const int NON_PERSISTENT_DELIVERYMODE = 1;
    private const int RABBITMQ_PREFETCH_SIZE = 0;
    private const int RABBITMQ_PREFETCH_COUNT = 10;
    private const bool RABBITMQ_GLOBAL_CONFIG = false;

    public QueueService(IConfiguration config)
    {
        var factory = new ConnectionFactory() { HostName = config.GetConnectionString("RabbitMq")! };

        _connection = factory.CreateConnection();

        _channel = _connection.CreateModel();

        _channel.BasicQos(RABBITMQ_PREFETCH_SIZE, RABBITMQ_PREFETCH_COUNT, RABBITMQ_GLOBAL_CONFIG);
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

    public void CreateTopic(Queue queue, Exchange exchange)
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

    public void CreateQueue(Queue queue)
    {
        try
        {
            QueueDeclare(queue.Name);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void SendMessage(QueueMessage message)
    {
        if (message.Exchange is not null)
            CreateTopic(message.Queue, message.Exchange);
        else
            CreateQueue(message.Queue);

        var messageBodyBytes = Encoding.UTF8.GetBytes(message.MessageBody);

        IBasicProperties props = _channel.CreateBasicProperties();

        props.DeliveryMode = NON_PERSISTENT_DELIVERYMODE;

        _channel.BasicPublish(message.Exchange?.Name ?? "", message.RoutingKey, props, messageBodyBytes);
    }


    public async Task ConsumeQueue(string queueName, Func<string, ulong, Task> messageHandlerAsync, CancellationToken cancellationToken)
    {
        var consumer = new EventingBasicConsumer(_channel);

        var message = string.Empty;

        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            message = Encoding.UTF8.GetString(body);

            try
            {
                await messageHandlerAsync(message, ea.DeliveryTag);

                _channel.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                _channel.BasicNack(ea.DeliveryTag, false, true);
            }
        };

        _channel.BasicConsume(queue: queueName,
                              autoAck: false,
                              consumer: consumer);

        cancellationToken.Register(() =>
        {
            foreach (var tag in consumer.ConsumerTags)
                _channel.BasicCancel(tag);
        });
    }


    public void Dispose()
    {
        _channel.Close();
        _connection.Close();
    }
}
