using RabbitEvents.Shared.Models.Messaging;

namespace RabbitEvents.Shared.Constants;

public static class QueueDefinitions
{
    public static Exchange AUTOR_EXCHANGE = new("autor_exchange", "topic");

    public static Queue AUTOR_QUEUE = new("autor_add_update", "autor.add_update");

    public static Queue AUTOR_IMAGE_QUEUE = new("autor_image", "autor.images");
}