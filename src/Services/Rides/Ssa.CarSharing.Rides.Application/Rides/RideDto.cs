using Ssa.CarSharing.Rides.Domain.Rides;

namespace Ssa.CarSharing.Rides.Application.Rides;

public record RideDto(Guid Id, DateTime StartsAtUtc, DateTime EndsAtUtc, string PickupCity, string DropOffCity, short NumberOfProposedSeats, short NumberOfAvailableSeats) {
    public static RideDto FromRide(Ride ride) { 
        return new RideDto(ride.Id, ride.StartsAtUtc, ride.EndsAtUtc, ride.PickupCity, ride.DropOffCity, ride.NumberOfProposedSeats, ride.NumberOfAvailableSeats);
    } 
}
