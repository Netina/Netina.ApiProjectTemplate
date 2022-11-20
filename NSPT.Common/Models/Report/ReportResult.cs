namespace Common.Models.Report;

public class ReportResult
{
    private List<ReportRow> _rows;

    public List<ReportRow> Rows
    {
        get
        {
            if (_rows == null)
                _rows = new List<ReportRow>();
            return _rows;
        }
        set => _rows = value;
    }
}

public class ReportRow
{
    private List<string> _cells;

    public List<string> Cells
    {
        get
        {
            if (_cells == null)
                _cells = new List<string>();
            return _cells;
        }
        set => _cells = value;
    }
}