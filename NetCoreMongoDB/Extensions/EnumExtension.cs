using NetCoreMongoDB.Models;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace NetCoreMongoDB.Extensions;

public static class EnumExtension
{
    public static EnumValue GetValue(this Enum enu)
    {
        if (enu == null)
            return null;
        var attr = GetDisplayAttribute(enu);
        return new EnumValue
        {
            Value = Convert.ToInt32(enu),
            Name = enu.ToString(),
            DisplayName = attr != null ? attr.Name : enu.ToString()
        };
    }

    public static EnumValue GetValue<T>(int value)
    {
        foreach (var itemType in Enum.GetValues(typeof(T)))
            if ((int)itemType == value)
            {
                var type = itemType.GetType();
                var field = type.GetField(itemType.ToString() ?? string.Empty);

                return new EnumValue
                {
                    Value = (int)itemType,
                    Name = Enum.GetName(typeof(T), itemType),
                    DisplayName = field != null ? field.GetCustomAttribute<DisplayAttribute>().Name : null
                };
            }

        return null;
    }

    private static DisplayAttribute GetDisplayAttribute(object value)
    {
        if (value == null)
            return null;
        var type = value.GetType();
        if (!type.IsEnum) throw new ArgumentException(string.Format("Type {0} is not an enum", type));

        // Get the enum field.
        var field = type.GetField(value.ToString() ?? string.Empty);
        return field == null ? null : field.GetCustomAttribute<DisplayAttribute>();
    }
}