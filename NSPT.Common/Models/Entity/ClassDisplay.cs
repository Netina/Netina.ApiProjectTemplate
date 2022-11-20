namespace Common.Models.Entity;

[AttributeUsage(AttributeTargets.Class)]
public class ClassDisplay : Attribute
{
    private readonly string _description;
    private readonly string _name;

    public ClassDisplay(string name, string description)
    {
        _name = name;
        _description = description;
    }

    public string GetName()
    {
        return _name;
    }

    public string GetDescription()
    {
        return _description;
    }
}