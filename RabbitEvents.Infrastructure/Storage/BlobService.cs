using Azure.Storage.Blobs.Models;

namespace RabbitEvents.Infrastructure.Storage;

public sealed class BlobService : IBlobService
{
    //https://www.youtube.com/watch?v=Ft4SJgQETAk&t=81s
    // olhar documentação

    private readonly BlobServiceClient _blobServiceClient;

    public BlobService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task<string> UploadFileAsync(string containerName, Stream stream, string fileName, string contentType, CancellationToken cancellationToken = default)
    {
        BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);

        BlobClient blobClient = blobContainerClient.GetBlobClient(fileName);

        var blobOptions = new BlobUploadOptions()
        {
            HttpHeaders = new BlobHttpHeaders { ContentType = contentType }
        };
        ;
        await blobClient.UploadAsync(
            stream,
            blobOptions,
            cancellationToken
        );

        return fileName;
    }


    public Task DeleteFileAsync(string fileName, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
