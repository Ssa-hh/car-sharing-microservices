using Ssa.CarSharing.Common.Application.CQRS;

namespace Ssa.CarSharing.Rides.Application.Rides.Commands.CreateRide;

public record CreateRideCommand(CarDto Car, short NumberOfProposedSeats, DateTime StartsAtUtc, DateTime EndsAtUtc, string PickupCity, string DropOffCity) : ICommand<Guid>;
