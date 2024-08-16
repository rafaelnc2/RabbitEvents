namespace RabbitEvents.Shared.Models.Messaging;

public record Queue(string Name, string RoutingKey, Dictionary<string, object>? QueueArguments = null);
