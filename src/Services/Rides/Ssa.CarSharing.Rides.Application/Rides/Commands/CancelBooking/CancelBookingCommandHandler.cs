using Ssa.CarSharing.Common.Application.Authentication;
using Ssa.CarSharing.Common.Application.CQRS;
using Ssa.CarSharing.Common.Domain;
using Ssa.CarSharing.Rides.Domain.Members;
using Ssa.CarSharing.Rides.Domain.Rides;

namespace Ssa.CarSharing.Rides.Application.Rides.Commands.CancelBooking;

internal class CancelBookingCommandHandler : ICommandHandler<CancelBookingCommand>
{
    private readonly IRideRepository _rideRepository;
    private readonly IUserContext _userContext;
    private readonly IMemberRepository _memberRepository;

    public CancelBookingCommandHandler(IRideRepository rideRepository, IUserContext userContext, IMemberRepository memberRepository)
    {
        _rideRepository = rideRepository;
        _userContext = userContext;
        _memberRepository = memberRepository;
    }

    public async Task<Result> Handle(CancelBookingCommand command, CancellationToken cancellationToken)
    {
        string currentMemberEmail = _userContext.UserEmail;

        Member? currentMember = await _memberRepository.GetByEmailAsync(currentMemberEmail);

        if (currentMember == null)
            return Result.Failure(Error.NotFound("Members.NotFound", "Current member not found."));

        Ride? ride = await _rideRepository.GetByIdAsync(command.RideId, cancellationToken);

        if (ride is null)
            return Result.Failure(Error.NotFound("Rides.NotFound", "Requested ride not found."));

        Booking? memberBooking = ride.Bookings.FirstOrDefault(b => b.Passenger.Id == currentMember.Id);

        if (memberBooking is null)
            return Result.Failure(Error.NotFound("Bookings.NotFound", "You don't have a booking for this ride."));

        Result result = ride.CancelBooking(memberBooking.Id);

        if (result.IsFailure)
            return result;

        var nbrModified = await _rideRepository.ReplaceAsync(ride, cancellationToken);

        if (nbrModified == 0)
            return Result.Failure<Guid>(Error.NotFound("Rides.FailUpdate", "Fail to update database."));

        return Result.Success();
    }
}
