using Microsoft.AspNetCore.Http;

namespace RabbitEvents.Application.Extensions;

public static class FormFileExtensions
{
    public static string GetFileExtension(this IFormFile formFile)
    {
        if (formFile is null)
            return string.Empty;

        var contentTypeSpan = formFile!.ContentType.AsSpan();

        var slashIndex = contentTypeSpan.IndexOf("/");

        var fileExtention = contentTypeSpan.Slice(slashIndex + 1);

        return fileExtention.ToString();
    }

    public static string GetBase64File(this IFormFile formFile)
    {
        if (formFile is null)
            return string.Empty;

        using Stream? stream = formFile.OpenReadStream();

        byte[] bytes;
        using (var memoryStream = new MemoryStream())
        {
            stream.CopyTo(memoryStream);
            bytes = memoryStream.ToArray();
        }

        return Convert.ToBase64String(bytes);
    }

    public static byte[]? GetByteArray(this IFormFile formFile)
    {
        if (formFile is null)
            return null;

        using Stream? stream = formFile.OpenReadStream();

        byte[] fileBytes;
        using (var memoryStream = new MemoryStream())
        {
            stream.CopyTo(memoryStream);
            fileBytes = memoryStream.ToArray();
        }

        return fileBytes;
    }
}
