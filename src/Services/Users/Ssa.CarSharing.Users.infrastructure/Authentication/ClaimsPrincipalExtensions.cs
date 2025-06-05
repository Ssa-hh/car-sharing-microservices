using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ssa.CarSharing.Users.infrastructure.Authentication
{
    internal static class ClaimsPrincipalExtensions
    {
        public static string GetUserEmail(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal?.FindFirstValue(ClaimTypes.Email) ?? throw new ApplicationException("User email unavailable");
        }

        public static string GetIdentityId(this ClaimsPrincipal claimsPrincipal) 
        {
            return  claimsPrincipal?.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new ApplicationException("User identity id unavailable");
        }
    }
}
