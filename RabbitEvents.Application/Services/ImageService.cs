using Microsoft.AspNetCore.Http;
using RabbitEvents.Application.Interfaces.Events;

namespace RabbitEvents.Application.Services;

public sealed class ImageService
{
    private readonly ILogger<ImageService> _logger;
    private readonly ICacheService _cacheService;
    private readonly IBus _bus;

    public ImageService(ILogger<ImageService> logger, ICacheService cacheService, IBus bus)
    {
        _logger = logger;
        _cacheService = cacheService;
        _bus = bus;
    }

    public async Task SaveImageService(IFormFile? image, string prefixCacheKey, Guid entityId, string name, IIntegrationEvent eventWithImage)
    {
        if (image is null)
            return;

        var fileExtension = image.GetFileExtension();
        await _cacheService.SetValueAsync(
            $"{prefixCacheKey}{CacheKeysConstants.KEY_SEPARATOR}{entityId}",
            image.GetByteArray()!,
            CacheKeysConstants.DEFAULT_EXPIRES
        )!;

        await _bus.Publish((object)eventWithImage);
    }

    public async Task SaveImageService(IFormFile? image, string prefixCacheKey, Guid entityId, string name,
        IIntegrationEvent eventWithImage, IIntegrationEvent eventToCreateImage)
    {
        if (image is null)
        {
            await _bus.Publish((object)eventToCreateImage);

            return;
        }

        var fileExtension = image.GetFileExtension();
        await _cacheService.SetValueAsync(
            $"{prefixCacheKey}{CacheKeysConstants.KEY_SEPARATOR}{entityId}",
            image.GetByteArray()!,
            CacheKeysConstants.DEFAULT_EXPIRES
        )!;

        await _bus.Publish((object)eventWithImage);
    }
}
