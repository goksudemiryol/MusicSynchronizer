namespace MusicSynchronizer.Common.Extensions;

public static class ObjectExtensions
{
    public static Dictionary<string, object?> GetPropertyValues(this object value)
    {
        var properties = new Dictionary<string, object?>();

        var propertyInfo = value.GetType().GetProperties();

        foreach (var item in propertyInfo)
        {
            properties.Add(item.Name, item.GetValue(value));
        }

        return properties;
    }
}
