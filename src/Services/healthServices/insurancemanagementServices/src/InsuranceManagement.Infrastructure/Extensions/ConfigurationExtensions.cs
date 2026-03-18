using Microsoft.Extensions.Configuration;

namespace InsuranceManagement.Infrastructure.Extensions;

/// <summary>
/// Extension methods for IConfiguration
/// </summary>
public static class ConfigurationExtensions
{
    /// <summary>
    /// Bind configuration section to object
    /// </summary>
    public static void Bind(this IConfigurationSection section, object instance)
    {
        if (section == null) throw new ArgumentNullException(nameof(section));
        if (instance == null) throw new ArgumentNullException(nameof(instance));

        var type = instance.GetType();
        var properties = type.GetProperties();

        foreach (var property in properties)
        {
            if (!property.CanWrite) continue;

            var value = section[property.Name];
            if (value == null) continue;

            try
            {
                if (property.PropertyType == typeof(int))
                    property.SetValue(instance, int.Parse(value));
                else if (property.PropertyType == typeof(bool))
                    property.SetValue(instance, bool.Parse(value));
                else if (property.PropertyType == typeof(decimal))
                    property.SetValue(instance, decimal.Parse(value));
                else if (property.PropertyType == typeof(long))
                    property.SetValue(instance, long.Parse(value));
                else if (property.PropertyType == typeof(string))
                    property.SetValue(instance, value);
            }
            catch
            {
                // Ignore conversion errors
            }
        }
    }

    /// <summary>
    /// Get configuration value with default
    /// </summary>
    public static T GetValue<T>(this IConfiguration configuration, string key, T defaultValue)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));
        if (key == null) throw new ArgumentNullException(nameof(key));

        var value = configuration[key];
        if (value == null) return defaultValue;

        try
        {
            if (typeof(T) == typeof(int))
                return (T)(object)int.Parse(value);
            else if (typeof(T) == typeof(bool))
                return (T)(object)bool.Parse(value);
            else if (typeof(T) == typeof(decimal))
                return (T)(object)decimal.Parse(value);
            else if (typeof(T) == typeof(long))
                return (T)(object)long.Parse(value);
            else if (typeof(T) == typeof(string))
                return (T)(object)value;
        }
        catch
        {
            return defaultValue;
        }

        return defaultValue;
    }

    /// <summary>
    /// Check if configuration section exists
    /// </summary>
    public static bool Exists(this IConfigurationSection section)
    {
        return section?.Value != null || (section?.GetChildren().Any() ?? false);
    }
}
