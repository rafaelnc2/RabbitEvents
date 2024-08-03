namespace RabbitEvents.Application.Interfaces;

public interface IBlobService
{
    Task<string> UploadFileAsync(string containerName, Stream stream, string fileName, string contentType, CancellationToken cancellationToken = default);

    Task DeleteFileAsync(string fileName, CancellationToken cancellationToken = default);
}
