using RabbitEvents.Shared.Models.Messaging;

namespace RabbitEvents.Shared.Constants;

public static class QueueDefinitions
{
    public static Exchange AUTOR_EXCHANGE = new("autor_exchange", "topic");

    public static Queue AUTOR_ADD_QUEUE = new("autor_add_queue", "autor_add_route");
    //public static string AUTOR_ADD_ROUTINGKEY = "autor_*"; //testar com curinga - tudo o que tiver "autor_" na rk deve chegar também nesta fila

    public static Queue AUTOR_IMAGE_QUEUE = new("autor_image_queue", "autor_image_route");
    //public static string AUTOR_IMAGE_ROUTINGKEY = "*autor_image*"; //testar com curinga - tudo o que tiver "autor_image" na rk deve chegar também nesta fila
}