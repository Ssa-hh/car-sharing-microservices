using Ssa.CarSharing.Users.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ssa.CarSharing.Users.Application.Abstructions
{
    public interface IAuthenticationService
    {
        Task<string> RegisterUser(User user, string password, CancellationToken cancellationToken = default);
    }
}
