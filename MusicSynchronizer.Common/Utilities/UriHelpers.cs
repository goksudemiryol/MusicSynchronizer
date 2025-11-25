using Microsoft.AspNetCore.WebUtilities;

namespace MusicSynchronizer.Common.Utilities;

public static class UriHelpers
{
    /// <summary>
    /// Appends the given <paramref name="name"/> - <paramref name="value"/> pair to the ref string parameter <paramref name="uri"/> using the method <see cref="QueryHelpers.AddQueryString"/>.
    /// </summary>
    /// <param name="uri">The Uri string to be appended.</param>
    /// <param name="name">The query parameter name.</param>
    /// <param name="value">The query parameter value.</param>
    public static void AppendQueryParameter(ref string uri, string name, string value)
    {
        uri = QueryHelpers.AddQueryString(uri, name, value);
    }

    public static void AppendQueryParameter(ref string uri, string name, object value)
    {
        uri = QueryHelpers.AddQueryString(uri, name, value.ToString());
    }
}
