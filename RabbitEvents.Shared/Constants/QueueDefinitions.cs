using RabbitEvents.Shared.Models.Messaging;

namespace RabbitEvents.Shared.Constants;

public static class QueueDefinitions
{
    public static Exchange AUTHORS_EXCHANGE = new("authors_exchange", "topic");

    public static Exchange IMAGES_EXCHANGE = new("images_exchange", "topic");


    public static Queue AUTHORS_QUEUE = new("authors", "authors");

    public static Queue IMAGES_ADD_UPDATE_QUEUE = new("images_add_update", "images.add_update");

    public static Queue IMAGES_CREATE_QUEUE = new("images_create", "images.create");
}