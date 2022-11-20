using System.IdentityModel.Tokens.Jwt;

namespace Common.Models.Api;

public class AccessToken
{
    public AccessToken()
    {
    }

    public AccessToken(JwtSecurityToken securityToken)
    {
        access_token = new JwtSecurityTokenHandler().WriteToken(securityToken);
        token_type = "Bearer";
        expire_in_datetime = securityToken.ValidTo;
        expires_in = (int)(securityToken.ValidTo - DateTime.UtcNow).TotalSeconds;
    }

    public string access_token { get; set; } = string.Empty;
    public string refresh_token { get; set; } = string.Empty;
    public string token_type { get; set; } = string.Empty;
    public int expires_in { get; set; }
    public string token_id { get; set; } = string.Empty;
    public DateTime expire_in_datetime { get; set; }
    public string BearerToken => $"Bearer {access_token}";
}

public class AccessToken<TUser>
{
    public AccessToken()
    {
    }

    public AccessToken(JwtSecurityToken securityToken)
    {
        access_token = new JwtSecurityTokenHandler().WriteToken(securityToken);
        token_type = "Bearer";
        expires_in = (int)(securityToken.ValidTo - DateTime.UtcNow).TotalSeconds;
    }

    public string access_token { get; set; } = string.Empty;
    public string ig_access_token { get; set; } = string.Empty;
    public string refresh_token { get; set; } = string.Empty;
    public string token_type { get; set; } = string.Empty;
    public int expires_in { get; set; }
    public TUser User { get; set; }

    public string BearerToken => $"Bearer {access_token}";
}