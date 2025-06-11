using Ssa.CarSharing.Users.Domain.Users;

namespace Ssa.CarSharing.Users.Application.Abstractions;

public interface IAuthenticationService
{
    Task<string> RegisterUser(User user, string password, CancellationToken cancellationToken = default);
}
