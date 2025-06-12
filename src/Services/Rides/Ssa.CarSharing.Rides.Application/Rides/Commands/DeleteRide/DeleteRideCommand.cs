using Ssa.CarSharing.Common.Application.CQRS;

namespace Ssa.CarSharing.Rides.Application.Rides.Commands.DeleteRide;

public record DeleteRideCommand(Guid rideId): ICommand;
