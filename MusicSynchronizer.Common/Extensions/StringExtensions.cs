using System.Text.RegularExpressions;
using System.Xml;

namespace MusicSynchronizer.Common.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Tries to create a <see cref="Uri"/> with the string and throws an exception if the string is not in the correct format.
    /// </summary>
    /// <param name="uriString"></param>
    /// <returns>The created <see cref="Uri"/> instance.</returns>
    public static Uri ToUri(this string uriString)
    {
        return ToUri(uriString, $"The value \"{uriString}\" is not a valid URL.", UriKind.RelativeOrAbsolute);
    }

    public static Uri ToUri(this string uriString, string exceptionMessage)
    {
        return ToUri(uriString, exceptionMessage, UriKind.RelativeOrAbsolute);
    }

    public static Uri ToUri(this string uriString, UriKind uriKind)
    {
        return ToUri(uriString, $"The value \"{uriString}\" is not a valid URL.", uriKind);
    }

    public static Uri ToUri(this string uriString, string exceptionMessage, UriKind uriKind)
    {
        return Uri.TryCreate(uriString, uriKind, out var uri)
            ? uri
            : throw new Exception(exceptionMessage);
    }

    public static bool IsUriString(this string? uriString)
    {
        return Uri.TryCreate(uriString, UriKind.RelativeOrAbsolute, out _);
    }

    public static TimeSpan ConvertIso8601ToTimeSpan(this string iso8601Duration)
    {
        return XmlConvert.ToTimeSpan(iso8601Duration);
    }

    public static int ConvertIso8601ToMilliSeconds(this string iso8601Duration)
    {
        return (int)XmlConvert.ToTimeSpan(iso8601Duration).TotalMilliseconds;
    }

    /// <summary>
    /// Removes all non-Unicode letters and digits from a string (including white spaces).
    /// </summary>
    /// <param name="value">String whose characters are to be removed.</param>
    /// <returns>The normalized string.</returns>
    public static string NormalizeForComparison(this string value)
    {
        //Remove NonUnicode Letters and Digits
        return Regex.Replace(value, @"[^\p{L}\p{N}\p{M}]+", "");
    }

    /// <summary>
    /// Normalizes the strings with the method <see cref="NormalizeForComparison"/> and checks whether <paramref name="value"/> contains <paramref name="isContained"/> with invariant culture and ignored case.
    /// </summary>
    /// <param name="value">Containing string.</param>
    /// <param name="isContained">String to check whether it is present.</param>
    /// <returns><see langword="true"/> if the normalized parameter <paramref name="value"/> contains the normalized parameter <paramref name="isContained"/>; otherwise, <see langword="false"/>.</returns>
    public static bool ContainsNormalized(this string value, string isContained)
    {
        return value.NormalizeForComparison().Contains(isContained.NormalizeForComparison(), StringComparison.InvariantCultureIgnoreCase);
    }
}
