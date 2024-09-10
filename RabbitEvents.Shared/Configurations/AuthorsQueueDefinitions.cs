using RabbitEvents.Shared.Models.Messaging;

namespace RabbitEvents.Shared.Configurations;

public static partial class QueueDefinitions
{
    // EXCHANGE DECLARE
    public static Exchange AUTHORS_EXCHANGE = new("authors_exchange", "topic");

    public static Exchange AUTHOR_DLQ_EXCHANGE = new("author_dlq_exchange", "direct");

    // AUTHOR QUEUE DECLARE
    public static Queue AUTHORS_QUEUE = new("authors", "authors");

    public static Queue AUTHORS_IMAGE_UPDATE_QUEUE = new("authors_image_update", "authors.image.update");
}
