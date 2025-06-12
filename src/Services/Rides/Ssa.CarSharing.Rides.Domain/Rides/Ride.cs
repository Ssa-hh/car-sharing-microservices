using Ssa.CarSharing.Common.Domain;
using Ssa.CarSharing.Rides.Domain.Members;

namespace Ssa.CarSharing.Rides.Domain.Rides;

public class Ride:AggregateRoot
{
    private readonly List<Booking> _bookings = new List<Booking>(); 
    public Ride(Guid id, Member driver, Car car, short numberOfProposedSeats, DateTime startsAtUtc, DateTime endsAtUtc, string pickupCity, string dropOffCity) : base(id)
    {
        Driver = driver;
        Car = car;
        NumberOfProposedSeats = numberOfProposedSeats;
        StartsAtUtc = startsAtUtc;
        EndsAtUtc = endsAtUtc;
        PickupCity = pickupCity;
        DropOffCity = dropOffCity;
        Status = RideStatus.Open;
        _bookings = new List<Booking>();
    }

    public Member Driver { get; private set; }

    public Car Car { get; private set; }

    public short NumberOfProposedSeats { get; private set; }

    public DateTime StartsAtUtc { get; private set; }

    public DateTime EndsAtUtc { get; private set; }

    public RideStatus Status { get; private set; }

    public string PickupCity { get; private set; }

    public string DropOffCity { get; private set; }

    public IReadOnlyList<Booking> Bookings => _bookings.AsReadOnly();

    public static Ride Create(Member driver, Car car, short numberOfProposedSeats, DateTime startsAtUtc, DateTime endsAtUtc, string pickupCity, string dropOffCity) {
        if (driver == null) throw new ArgumentNullException("Driver is required.");
        if (car == null) throw new ArgumentNullException("Car is required.");
        if (numberOfProposedSeats <= 0) throw new ArgumentOutOfRangeException("Number of proposed seats must be greater than 0.");
        if (startsAtUtc >= endsAtUtc) throw new ArgumentOutOfRangeException("Arrival time must be greater than departure time.");
        if (string.IsNullOrWhiteSpace(pickupCity)) throw new ArgumentNullException("Pick up city is required.");
        if (string.IsNullOrWhiteSpace(dropOffCity)) throw new ArgumentNullException("Drop off city is required.");
        
        return new Ride(Guid.NewGuid(), driver, car, numberOfProposedSeats, startsAtUtc, endsAtUtc, pickupCity, dropOffCity);
    }

    public Result AddBooking(Booking booking)
    {
        bool memberHasBooking = _bookings.Any(b => b.Passenger.Id == booking.Passenger.Id);
        if (memberHasBooking)
            return Result.Failure(Error.Conflict("Bookings.Conflict", $"{booking.Passenger.FirstName} {booking.Passenger.LastName} is already booked for this ride."));

        _bookings.Add(booking);
        return Result.Success();
    }

    public Result RemoveBooking(Guid bookingId)
    {
        Booking? bookingToDelete = _bookings.Where(b => b.Id == bookingId).FirstOrDefault();
        if (bookingToDelete is null)
            return Result.Failure(Error.NotFound("Bookings.NotFound", "Booking not found"));

        _bookings.Remove(bookingToDelete);
        return Result.Success();
    }
}
