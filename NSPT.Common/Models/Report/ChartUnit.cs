namespace Common.Models.Report;

public class ChartUnit
{
    public List<long> Values { get; set; } = new();

    public List<string> ValuesStr
    {
        get { return Values.Select(v => v.ToString()).ToList(); }
    }

    public List<string> Labels { get; set; } = new();
}

public class ChartUnitIQuery
{
    public IQueryable<long> Values { get; set; }

    public List<string> ValuesStr
    {
        get { return Values.Select(v => v.ToString()).ToList(); }
    }

    public IQueryable<string> Labels { get; set; }
}

public class ChartUnit<TValue>
{
    public List<TValue> Values { get; set; } = new();

    public List<string> ValuesStr
    {
        get { return Values.Select(v => v.ToString()).ToList(); }
    }

    public List<string> Labels { get; set; } = new();
}