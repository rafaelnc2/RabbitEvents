using RabbitEvents.Shared.Models.Messaging;

namespace RabbitEvents.Shared.Configurations;

public static partial class QueueDefinitions
{
    public static int MAX_RETY_COUNT = 5;

    // EXCHANGE DECLARE
    public static Exchange IMAGES_EXCHANGE = new("images_exchange", "topic");

    public static Exchange IMAGES_DLQ_EXCHANGE = new("image_dlq_exchange", "direct");

    //DEAD LETTER DECLARE
    public static Queue IMAGES_DLQ_QUEUE = new("images_dlq", "#");

    private static Dictionary<string, object> DeadLetterImageQueueArguments = new()
    {
        { "x-dead-letter-exchange", IMAGES_DLQ_EXCHANGE!.Name }
    };

    public static Queue IMAGES_UPLOAD_QUEUE = new("images_upload", "images.upload", DeadLetterImageQueueArguments);

    public static Queue IMAGES_CREATE_QUEUE = new("images_create", "images.create", DeadLetterImageQueueArguments);
}