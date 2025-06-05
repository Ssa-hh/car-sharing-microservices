using Ssa.CarSharing.Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ssa.CarSharing.Users.Application.Abstructions
{
    public interface IJwtService
    {
        Task<Result<string>> GetClientAccessTokenAsync(string email, string password, CancellationToken cancellationToken = default);

        Task<Result<string>> GetAdminAccessTokenAsync(CancellationToken cancellationToken = default);
    }
}
