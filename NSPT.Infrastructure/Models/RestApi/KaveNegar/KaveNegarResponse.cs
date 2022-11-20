namespace Infrastructure.Models.RestApi.KaveNegar;

public class KaveNegarResponse
{
    public KaveNegarReturn Return { get; set; }
    public KaveNegarResponseEntry[] entries { get; set; }
}