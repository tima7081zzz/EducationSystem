using System.ComponentModel;

namespace Core.Helpers;

public static class EnumUtils
{
    public static string GetEnumValueDescription(Enum value)
    {
        var stringValue = value.ToString();
        
        return (value
            .GetType()
            .GetField(stringValue)
            ?.GetCustomAttributes(typeof(DescriptionAttribute), false)
            .FirstOrDefault() as DescriptionAttribute)?.Description ?? stringValue;
    }
}