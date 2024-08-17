namespace RabbitEvents.Shared.Models.Messaging;

public sealed record QueueMessage(Queue Queue, Exchange? Exchange, string RoutingKey, string MessageBody);