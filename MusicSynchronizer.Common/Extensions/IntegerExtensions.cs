namespace MusicSynchronizer.Common.Extensions;

public static class IntegerExtensions
{
    /// <summary>
    /// Returns whether the error code is a client error code (4xx).
    /// </summary>
    /// <param name="statusCode">The error code.</param>
    /// <returns><see langword="true"/> if the <paramref name="statusCode"/> is a client error code; otherwise, <see langword="false"/>.</returns>
    public static bool IsClientError(this int statusCode)
    {
        return statusCode >= 400 && statusCode < 500;
    }
}
