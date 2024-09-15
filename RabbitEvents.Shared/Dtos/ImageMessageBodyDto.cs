using RabbitEvents.Shared.Models.Messaging;

namespace RabbitEvents.Shared.Dtos;

public record ImageMessageBodyDto(
    string ImageId,
    string FileExtension,
    string ContentType,
    string BlobContainerName,
    Queue DestinationQueue,
    Exchange? DestinationExchange
);
