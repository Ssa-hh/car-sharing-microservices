using Ssa.CarSharing.Common.Application.CQRS;
using Ssa.CarSharing.Rides.Domain.Rides;

namespace Ssa.CarSharing.Rides.Application.Rides.Commands.BookRide;

public record BookRideCommand(Guid RideId, short NumberOfRequestedSeats) :ICommand<Guid>;
