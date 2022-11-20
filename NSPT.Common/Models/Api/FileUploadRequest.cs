namespace Common.Models.Api;

public enum FileUploadType
{
    Handout,
    Video,
    Image
}
public class FileUploadRequest
{
    public string StringBaseFile { get; set; }
    public string FileName { get; set; }
    public FileUploadType FileUploadType { get; set; }
}
