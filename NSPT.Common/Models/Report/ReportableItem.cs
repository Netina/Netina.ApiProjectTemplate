using System.Collections;
using System.Reflection;

namespace Common.Models.Report;

public interface IReportableItem
{
    string Name { get; set; }
    string PropertyName { get; set; }
    object DefaultValue { get; set; }
    object SelectedValue { get; set; }
    string ItemType { get; }
    object Element { get; set; }
}

public class BoolReportable : IReportableItem
{
    public BoolReportable()
    {
        ItemType = GetType().Name;
    }

    public string Name { get; set; }
    public string PropertyName { get; set; }
    public object DefaultValue { get; set; }
    public object SelectedValue { get; set; }
    public string ItemType { get; }
    public object Element { get; set; }
}

public class ListReportable : IReportableItem
{
    public ListReportable()
    {
        ItemType = GetType().Name;
    }

    public IList List { get; set; }
    public IList ConvertedList { get; set; }
    public string DisplayMemberPath { get; set; }
    public string SelectedMemberPath { get; set; }
    public string ListItemType { get; set; }
    public string Name { get; set; }
    public string PropertyName { get; set; }
    public object DefaultValue { get; set; }
    public object SelectedValue { get; set; }
    public string ItemType { get; }
    public object Element { get; set; }
}

public class EnumReportable : IReportableItem
{
    public EnumReportable()
    {
        ItemType = GetType().Name;
    }

    public string EnumTypeName { get; set; }

    public string Name { get; set; }
    public string PropertyName { get; set; }
    public object DefaultValue { get; set; }
    public object SelectedValue { get; set; }
    public string ItemType { get; }
    public object Element { get; set; }

    public Type EnumType(Assembly domainAssembly)
    {
        var types = domainAssembly.GetTypes();
        var type = types.FirstOrDefault(t => t.Name == EnumTypeName);
        return type;
    }
}

public class NumericReportable : IReportableItem
{
    public NumericReportable()
    {
        ItemType = GetType().Name;
    }

    public string Name { get; set; }
    public string PropertyName { get; set; }
    public object DefaultValue { get; set; }
    public object SelectedValue { get; set; }
    public string ItemType { get; }
    public object Element { get; set; }
}