using Ssa.CarSharing.Common.Application.CQRS;

namespace Ssa.CarSharing.Rides.Application.Rides.Commands.CancelBooking;

public record CancelBookingCommand(Guid RideId):ICommand;
