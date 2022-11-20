namespace Api.Controllers.V1;
[ApiVersion("1")]
[AllowAnonymous]
public class AuthController : BaseController
{

    [HttpPost("[action]")]
    public async Task<IActionResult> LoginSwagger([FromForm] TokenRequest tokenRequest, CancellationToken cancellationToken)
    {
        return new JsonResult(null);
    }
}
