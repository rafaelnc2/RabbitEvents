using RabbitEvents.Shared.Models.Messaging;

namespace RabbitEvents.Shared.Constants;

public static class QueueDefinitions
{
    public static Exchange AUTHORS_EXCHANGE = new("authors_exchange", "topic");

    public static Exchange IMAGES_EXCHANGE = new("images_exchange", "topic");


    public static Queue AUTHORS_QUEUE = new("authors", "authors");

    public static Queue AUTHORS_IMAGE_UPDATE_QUEUE = new("authors_image_update", "authors.image.update");

    public static Queue IMAGES_UPLOAD_QUEUE = new("images_upload", "images.upload");

    public static Queue IMAGES_CREATE_QUEUE = new("images_create", "images.create");
}