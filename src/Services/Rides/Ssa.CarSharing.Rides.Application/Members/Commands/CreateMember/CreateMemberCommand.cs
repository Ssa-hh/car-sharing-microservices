using Ssa.CarSharing.Common.Application.CQRS;

namespace Ssa.CarSharing.Rides.Application.Members.Commands.CreateMember;

public record CreateMemberCommand(string FirstName, string LastName, string Email): ICommand<Guid>;
