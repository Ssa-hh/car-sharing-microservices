using Ssa.CarSharing.Common.Domain;

namespace Ssa.CarSharing.Common.Application.Authentication;

public interface IJwtService
{
    Task<Result<string>> GetClientAccessTokenAsync(string email, string password, CancellationToken cancellationToken = default);
}
