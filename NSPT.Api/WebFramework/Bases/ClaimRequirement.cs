namespace Server.WebFramework.Bases;
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class ClaimRequirement : AuthorizeAttribute, IAuthorizationFilter
{
    private readonly string[] _claimsRequire;

    public ClaimRequirement(params string[] claimsRequire)
    {
        _claimsRequire = claimsRequire;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
        var permissions = user.Claims?.Where(c => c.Type == CustomClaimType.Permission)?.ToList();
        if (permissions == null)
        {
            context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
        }
        else
        {
            bool isAccepted=false;
            foreach (var claim in _claimsRequire)
            {
                if (permissions.FirstOrDefault(p => p.Value == claim)!=null)
                    isAccepted=true;

            }
            if(!isAccepted)
                context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
        }

        context.HttpContext.Request.Headers.Add(new KeyValuePair<string, StringValues>("IsAdminReuirmentPast", "True"));
    }
}
