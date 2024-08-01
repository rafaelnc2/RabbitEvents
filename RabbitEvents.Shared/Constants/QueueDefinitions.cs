using RabbitEvents.Shared.Models.Messaging;

namespace RabbitEvents.Shared.Constants;

public static class QueueDefinitions
{
    public static Exchange AUTHORS_EXCHANGE = new("authors_exchange", "topic");

    public static Queue AUTHORS_QUEUE = new("authors", "authors");

    public static Queue IMAGES_QUEUE = new("images", "images");
}