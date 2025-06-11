using Microsoft.AspNetCore.Http;
using Ssa.CarSharing.Common.Application.Authentication;
using System.Security.Claims;


namespace Ssa.CarSharing.Common.infrastructure.Authentication;

internal class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string UserEmail => _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email) ?? throw new ApplicationException("User email unavailable");

    public string IdentityId => _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new ApplicationException("User identity id unavailable");

}
