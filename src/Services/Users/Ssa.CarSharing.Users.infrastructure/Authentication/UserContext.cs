using Microsoft.AspNetCore.Http;
using Ssa.CarSharing.Users.Application.Abstructions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ssa.CarSharing.Users.infrastructure.Authentication
{
    internal class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string UserEmail => _httpContextAccessor.HttpContext?.User.GetUserEmail() ?? throw new ApplicationException("User context is unavailable");

        public string IdentityId => _httpContextAccessor.HttpContext?.User.GetIdentityId() ?? throw new ApplicationException("User context is unavailable");
    }
}
