using Repos.Interfaces;
using System.Security.Claims;

namespace Api.Services;
public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    public string UserName => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);

    public int MedicalCenterId => _httpContextAccessor.HttpContext?.User?.FindFirstValue("MedicalCenterId") == null ? 0 : int.Parse(_httpContextAccessor.HttpContext?.User?.FindFirstValue("MedicalCenterId"));
}


