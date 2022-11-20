namespace Infrastructure.Models.RestApi.KaveNegar;

public class KaveNegarResponseEntry
{
    public int messageid { get; set; }
    public string message { get; set; }
    public int status { get; set; }
    public string statustext { get; set; }
    public string sender { get; set; }
    public string receptor { get; set; }
    public int date { get; set; }
    public int cost { get; set; }
}