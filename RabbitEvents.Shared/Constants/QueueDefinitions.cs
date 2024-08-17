using RabbitEvents.Shared.Models.Messaging;

namespace RabbitEvents.Shared.Constants;

public static class QueueDefinitions
{
    public static int MAX_RETY_COUNT = 5;

    public static Exchange AUTHORS_EXCHANGE = new("authors_exchange", "topic");


    public static Exchange IMAGES_EXCHANGE = new("images_exchange", "topic");

    public static Exchange IMAGES_DLQ_EXCHANGE = new("image_dlq_exchange", "topic");



    public static Queue AUTHORS_QUEUE = new("authors", "authors");

    public static Queue AUTHORS_IMAGE_UPDATE_QUEUE = new("authors_image_update", "authors.image.update");


    public static Queue IMAGES_DLQ_QUEUE = new("images_dlq", "#");

    public static Queue IMAGES_UPLOAD_QUEUE = new("images_upload", "images.upload", new Dictionary<string, object>()
    {
        { "x-dead-letter-exchange", IMAGES_DLQ_EXCHANGE.Name }
    });

    public static Queue IMAGES_CREATE_QUEUE = new("images_create", "images.create");
}