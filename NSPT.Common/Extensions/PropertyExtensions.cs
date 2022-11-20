using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Common.Extensions;

public static class PropertyExtensions
{
    public static string GetPropertyDisplayName(this MemberInfo propertyExpression)
    {
        var memberInfo = propertyExpression;
        var attr = memberInfo.GetCustomAttributes<DisplayAttribute>().FirstOrDefault();
        if (attr == null) return memberInfo.Name;

        return attr.Name;
    }
}