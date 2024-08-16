using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitEvents.Infrastructure.Messaging;

//Criar dead letter para enviar a mensagem após 5 tentivas de consumo

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

    private void QueueDeclare(Queue queue) =>
        _channel.QueueDeclare(queue: queue.Name,
                              durable: true,
                              exclusive: false,
                              autoDelete: false,
                              arguments: queue.QueueArguments);

    private void QueueBind(string queue, string exchange, string routingKey) =>
        _channel.QueueBind(queue: queue,
                           exchange: exchange,
                           routingKey: routingKey);

    public void CreateTopic(Queue queue, Exchange exchange)
    {
        try
        {
            ExchangeDeclare(exchange.Name, exchange.Type);

            QueueDeclare(queue);

            if (string.IsNullOrEmpty(exchange.Name) is false && string.IsNullOrEmpty(queue.RoutingKey) is false)
                QueueBind(queue.Name, exchange.Name, queue.RoutingKey);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void CreateDeadLetterQueue(Queue queue, Exchange exchange)
    {
        try
        {
            ExchangeDeclare(exchange.Name, exchange.Type);

            QueueDeclare(queue);

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
            QueueDeclare(queue);
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

    private void RequeueMessageToOriginalQueue(BasicDeliverEventArgs ea) =>
        _channel.BasicPublish(ea.Exchange, ea.RoutingKey, ea.BasicProperties, ea.Body);


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
            catch (Exception)
            {
                CheckIfRequeueOrSendToDLQ(ea);
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

    private void CheckIfRequeueOrSendToDLQ(BasicDeliverEventArgs ea)
    {
        var requeueOnError = (RetryCountReachedLimit(ea) is false);

        if (requeueOnError is true)
        {
            RequeueMessageToOriginalQueue(ea);

            _channel.BasicAck(ea.DeliveryTag, multiple: false);
        }
        else
        {
            _channel.BasicNack(ea.DeliveryTag, false, requeue: false);
        }
    }

    private bool RetryCountReachedLimit(BasicDeliverEventArgs ea)
    {
        if (ea.BasicProperties.Headers is not null && ea.BasicProperties.Headers.ContainsKey("retry-count"))
        {
            var currentRetryCount = (int)ea.BasicProperties.Headers["retry-count"];

            if (currentRetryCount == QueueDefinitions.MAX_RETY_COUNT)
                return true;

            ea.BasicProperties.Headers["retry-count"] = currentRetryCount + 1;

            return false;
        }

        ea.BasicProperties.Headers = new Dictionary<string, object>() {
            { "retry-count", 1 }
        };

        return false;
    }


    public void Dispose()
    {
        _channel.Close();
        _connection.Close();
    }
}
