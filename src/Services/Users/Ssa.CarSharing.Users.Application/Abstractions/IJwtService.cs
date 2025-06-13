using Ssa.CarSharing.Common.Domain;

namespace Ssa.CarSharing.Users.Application.Abstractions;

public interface IJwtService
{
    Task<Result<string>> GetClientAccessTokenAsync(string email, string password, CancellationToken cancellationToken = default);
}
