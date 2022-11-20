namespace Common.Models.Entity;

public interface IApiEntity
{
    string CreatedBy { get; set; }
    string ModifiedBy { get; set; }
    string RemovedBy { get; set; }
    DateTime CreatedAt { get; set; }
    DateTime RemovedAt { get; set; }
    DateTime ModifiedAt { get; set; }
    bool IsRemoved { get; set; }
}