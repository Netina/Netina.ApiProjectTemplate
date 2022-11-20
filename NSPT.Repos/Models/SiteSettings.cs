
namespace Repos.Models;

public class SiteSettings
{
    public JwtSettings JwtSettings { get; set; }
    public UserSetting UserSetting { get; set; }
    public SwaggerSetting SwaggerSetting { get; set; }
    public string BaseUrl { get; set; }
}

public class JwtSettings
{
    public string SecretKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpireAddDay { get; set; }
}

public class SwaggerSetting
{
    public string LicenseName { get; set; }
    public string Description { get; set; }
    public string ApiName { get; set; }
}

public class UserSetting
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string RoleName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
}