namespace Common.Models.Report;

public enum ReportType
{
    ByDate,
    ByValue,
    BySelected
}

public class ReportRequestProp
{
    public string PropertyName { get; set; }
    public string PropertyValue { get; set; }
}

public class ReportRequest
{
    public string TableType { get; set; }
    public ReportType ReportType { get; set; }
    public DateTime FromDateTime { get; set; }
    public DateTime ToDateTime { get; set; }
    public List<ReportRequestProp> ReportRequestProps { get; set; } = new();
}