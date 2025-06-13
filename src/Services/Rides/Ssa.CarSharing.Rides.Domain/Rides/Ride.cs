using Ssa.CarSharing.Common.Domain;
using Ssa.CarSharing.Rides.Domain.Members;

namespace Ssa.CarSharing.Rides.Domain.Rides;

public class Ride:AggregateRoot
{
    private List<Booking> _bookings; 
    private Ride(Guid id, Member driver, Car car, short numberOfProposedSeats, DateTime startsAtUtc, DateTime endsAtUtc, string pickupCity, string dropOffCity) : base(id)
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

    public short NumberOfAvailableSeats => (short)(NumberOfProposedSeats - Bookings.Sum(b => b.NumberOfRequestedSeats));

    // This adaptation is done for MongoDb
    public IReadOnlyList<Booking> Bookings { 
        get {
            if(_bookings is null) _bookings = new List<Booking>();
            return _bookings;
        }
        private set {
            _bookings = value is null ? new List<Booking>() : value.ToList();
        }
    }

    public static Ride Create(Member driver, Car car, short numberOfProposedSeats, DateTime startsAtUtc, DateTime endsAtUtc, string pickupCity, string dropOffCity) {
        if (driver == null) throw new ArgumentNullException("Driver is required.");
        if (car == null) throw new ArgumentNullException("Car is required.");
        if (numberOfProposedSeats <= 0) throw new ArgumentOutOfRangeException("Number of proposed seats must be greater than 0.");
        if (startsAtUtc >= endsAtUtc) throw new ArgumentOutOfRangeException("Arrival time must be greater than departure time.");
        if (string.IsNullOrWhiteSpace(pickupCity)) throw new ArgumentNullException("Pick up city is required.");
        if (string.IsNullOrWhiteSpace(dropOffCity)) throw new ArgumentNullException("Drop off city is required.");
        
        return new Ride(Guid.NewGuid(), driver, car, numberOfProposedSeats, startsAtUtc, endsAtUtc, pickupCity, dropOffCity);
    }

    public Result<Guid> Book(Member passenger, short numberOfRequestedSeats)
    {
        if(passenger is null) throw new ArgumentNullException(nameof(passenger));

        if (numberOfRequestedSeats < 1) throw new ArgumentOutOfRangeException("The number of requested seats must be at least one.");
        
        if (Status != RideStatus.Open)
            return Result.Failure<Guid>(Error.Failure("Rides.NotOpen", "Requested ride is not open for booking."));

        if (Bookings.Any(b => b.Passenger.Id == passenger.Id))
            return Result.Failure<Guid>(Error.Conflict("Booking.Conflict", "You've already booked this ride"));

        if (NumberOfAvailableSeats < numberOfRequestedSeats)
            return Result.Failure<Guid>(Error.Failure("Rides.OverBooking", "The number of seats requested exceeds the available seats."));
        

        Booking newBooking = Booking.Create(passenger, numberOfRequestedSeats);

        _bookings.Add(newBooking);

        return Result.Success(newBooking.Id);
    }

    public Result CancelBooking(Guid bookingId)
    {
        Booking? bookingToDelete = _bookings.Where(b => b.Id == bookingId).FirstOrDefault();
        if (bookingToDelete is null)
            return Result.Failure(Error.NotFound("Bookings.NotFound", "Booking not found"));

        _bookings.Remove(bookingToDelete);
        return Result.Success();
    }
}
