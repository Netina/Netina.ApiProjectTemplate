using Common.Models.Entity;

namespace Common.Extensions;

public static class ClassDisplayExtensions
{
    public static string GetDisplayAttributeName<T>()
    {
        var attrs =
            Attribute.GetCustomAttributes(typeof(T));

        foreach (var attr in attrs)
        {
            var displayAttribute = attr as ClassDisplay;
            if (displayAttribute == null)
                continue;
            return displayAttribute.GetName();
        }

        return null;
    }

    public static string GetDisplayAttributeDescription<T>()
    {
        var attrs =
            Attribute.GetCustomAttributes(typeof(T));

        foreach (var attr in attrs)
        {
            var displayAttribute = attr as ClassDisplay;
            if (displayAttribute == null)
                continue;
            return displayAttribute.GetDescription();
        }

        return null;
    }
}