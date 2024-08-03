namespace RabbitEvents.Infrastructure.IoC;

internal class ServicesBootstrapper
{
    public void ServicesRegister(IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<IQueueService, QueueService>();

        services.AddSingleton<IBlobService, BlobService>();
        services.AddSingleton(_ => new BlobServiceClient(config.GetConnectionString("BlobStorage")));

        services.AddScoped<IAutorDomainService, AutorService>();
    }
}
