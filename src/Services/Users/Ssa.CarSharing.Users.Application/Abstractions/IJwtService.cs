using Ssa.CarSharing.Common.Domain;
using Ssa.CarSharing.Users.Application.Dtos;

namespace Ssa.CarSharing.Users.Application.Abstractions;

public interface IJwtService
{
    Task<Result<AuthorizationTokenDto>> GetClientAccessTokenAsync(string email, string password, CancellationToken cancellationToken = default);
}
