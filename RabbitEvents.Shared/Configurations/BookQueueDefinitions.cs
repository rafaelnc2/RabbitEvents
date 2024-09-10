using RabbitEvents.Shared.Models.Messaging;

namespace RabbitEvents.Shared.Configurations;

public static partial class QueueDefinitions
{
    // EXCHANGE DECLARE
    public static Exchange BOOKS_EXCHANGE = new("books_exchange", "topic");

    public static Exchange BOOKS_DLQ_EXCHANGE = new("books_dlq_exchange", "direct");

    // QUEUE DECLARE
    public static Queue BOOKS_QUEUE = new("books", "books");

    public static Queue BOOKS_IMAGE_UPDATE_QUEUE = new("books_image_update", "books.image.update");
}
