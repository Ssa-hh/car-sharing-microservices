using Ssa.CarSharing.Common.Application.Authentication;
using Ssa.CarSharing.Common.Application.CQRS;
using Ssa.CarSharing.Common.Domain;
using Ssa.CarSharing.Rides.Domain.Members;
using Ssa.CarSharing.Rides.Domain.Rides;

namespace Ssa.CarSharing.Rides.Application.Rides.Commands.DeleteRide
{
    internal class DeleteRideCommandHandler : ICommandHandler<DeleteRideCommand>
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IRideRepository _rideRepository;
        private readonly IUserContext _userContext;

        public DeleteRideCommandHandler(IUserContext userContext, IMemberRepository memberRepository, IRideRepository rideRepository)
        {
            _userContext = userContext;
            _memberRepository = memberRepository;
            _rideRepository = rideRepository;
        }

        public async Task<Result> Handle(DeleteRideCommand command, CancellationToken cancellationToken)
        {
            string currentMemberEmail = _userContext.UserEmail;

            Member? currentMember = await _memberRepository.GetByEmailAsync(currentMemberEmail);

            if (currentMember is null)
                return Result.Failure(Error.NotFound("Members.NotFound", "Logged member not found."));

            Ride? rideToDelete = await _rideRepository.GetByIdAsync(command.rideId);

            if(rideToDelete is  null)
                return Result.Failure(Error.NotFound("Rides.NotFound", "Ride not found."));

            if(rideToDelete.Driver.Id != currentMember.Id)
                return Result.Failure(Error.Forbidden("Members.Forbidden", $"The ride with id \"{command.rideId}\" does not belong to current logged in member."));

            await _rideRepository.DeleteAsync(command.rideId);

            return Result.Success();
        }
    }
}
