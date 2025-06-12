using Ssa.CarSharing.Common.Application.Authentication;
using Ssa.CarSharing.Common.Application.CQRS;
using Ssa.CarSharing.Common.Domain;
using Ssa.CarSharing.Rides.Domain.Members;
using Ssa.CarSharing.Rides.Domain.Rides;
using System.Drawing;

namespace Ssa.CarSharing.Rides.Application.Rides.Commands.CreateRide;

internal class CreateRideCommandHandler : ICommandHandler<CreateRideCommand, Guid>
{
    private readonly IRideRepository _rideRepository;
    private readonly IUserContext _userContext;
    private readonly IMemberRepository _memberRepository;

    public CreateRideCommandHandler(IRideRepository rideRepository, IUserContext userContext, IMemberRepository memberRepository)
    {
        _rideRepository = rideRepository;
        _userContext = userContext;
        _memberRepository = memberRepository;
    }

    public async Task<Result<Guid>> Handle(CreateRideCommand command, CancellationToken cancellationToken)
    {
        string currentMemberEmail = _userContext.UserEmail;

        Member? currentMember = await _memberRepository.GetByEmailAsync(currentMemberEmail);

        if(currentMember == null)
            return Result.Failure<Guid>(Error.NotFound("Members.NotFound", "Current member not found."));

        Car car = new(command.Car.Brand, command.Car.Model, command.Car.ColorHExCode, command.Car.NumberOfSeats);

        Ride newRide = Ride.Create(currentMember, car, command.NumberOfProposedSeats, command.StartsAtUtc, command.EndsAtUtc, command.PickupCity, command.DropOffCity);

        await _rideRepository.AddAsync(newRide, cancellationToken);

        return Result.Success(newRide.Id);
    }
}
