namespace Common.Models.Api;

public class HealthCheck
{
    public bool Health { get; set; }
    public string Version { get; set; }
    public string TotalMemory { get; set; }
}