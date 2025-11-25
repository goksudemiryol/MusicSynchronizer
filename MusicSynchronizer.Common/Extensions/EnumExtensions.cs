using System.Text.Json;

namespace MusicSynchronizer.Common.Extensions;

public static class EnumExtensions
{
    /// <summary>
    /// Gets the camelCase format of an enum member.
    /// </summary>
    /// <param name="enumValue">The enum member.</param>
    /// <returns>The camelCase representation of the <paramref name="enumValue"/>.</returns>
    public static string ToCamelCaseString(this Enum enumValue)
    {
        return JsonNamingPolicy.CamelCase.ConvertName(enumValue.ToString());
    }
}
