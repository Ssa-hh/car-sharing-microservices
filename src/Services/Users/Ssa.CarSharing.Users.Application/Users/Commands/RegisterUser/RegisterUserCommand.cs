using Ssa.CarSharing.Common.Application.CQRS;

namespace Ssa.CarSharing.Users.Application.Users.Commands.RegisterUser
{
    public record RegisterUserCommand(string FirstName, string LastName, string Email, string Password) : ICommand<Guid>;
}
