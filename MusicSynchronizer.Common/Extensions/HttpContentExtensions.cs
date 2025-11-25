namespace MusicSynchronizer.Common.Extensions;

public static class HttpContentExtensions
{
    public static string ReadAsString(this HttpContent httpContent)
    {
        using var stream = httpContent.ReadAsStream();
        using var streamReader = new StreamReader(stream);
        return streamReader.ReadToEnd();
    }
}
