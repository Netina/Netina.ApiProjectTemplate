namespace Common.Models.Api;

public class TokenRequest
{
    public string grant_type { get; set; }
    public string username { get; set; }
    public string password { get; set; }
    public string refresh_token { get; set; }
    public string scope { get; set; }

    public string client_id { get; set; }
    public string client_secret { get; set; }

    public string login_hash { get; set; }
    public string device_id { get; set; }
    public string license_id { get; set; }
}