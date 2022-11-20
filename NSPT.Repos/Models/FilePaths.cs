namespace Repos.Models;
public static class FilePaths
{
    public static string Videos => $"{Directory.GetCurrentDirectory()}/wwwroot/Videos";
    public static string Voices => $"{Directory.GetCurrentDirectory()}/wwwroot/Voices";
    public static string Images => $"{Directory.GetCurrentDirectory()}/wwwroot/Images";
    public static string IdentityImages => $"{Directory.GetCurrentDirectory()}/wwwroot/IdentityImages";
    public static string BackUps => $"{Directory.GetCurrentDirectory()}/wwwroot/BackUps";
    public static string Logs => $"{Directory.GetCurrentDirectory()}/wwwroot/Logs";
}
