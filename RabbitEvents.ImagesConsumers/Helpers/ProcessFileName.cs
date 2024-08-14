namespace RabbitEvents.ImagesConsumers.Helpers;

public static class ProcessFileName
{
    public static string GetTextAfterSeparator(string text, char separator)
    {
        var currentFileNameSpan = text.AsSpan();

        var charIndex = currentFileNameSpan.IndexOf(separator);

        var newFileName = currentFileNameSpan[(charIndex + 1)..];

        return newFileName.ToString();
    }
}
