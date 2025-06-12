using Ssa.CarSharing.Common.Domain;
using Ssa.CarSharing.Rides.Domain.Members;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ssa.CarSharing.Rides.Domain.Rides
{
    public class Booking : Entity
    {
        private Booking(Guid id, Member passenger, short numberOfBookedSeats, BookingStatus status):base(id)
        {
            Passenger = passenger;
            NumberOfBookedSeats = numberOfBookedSeats;
            Status = status;
        }

        public Member Passenger { get; private set; }

        public short NumberOfBookedSeats { get; private set; }

        public BookingStatus Status { get; private set; }

        internal static Booking Create(Member passenger, short numberOfBookedSeats, BookingStatus status = BookingStatus.Requested)
        {
            if (passenger == null) throw new ArgumentNullException("The passenger is required.");
            if (numberOfBookedSeats <= 0) throw new ArgumentOutOfRangeException("The number of requested seats must be greater than 0.");

            return new Booking(Guid.NewGuid(), passenger, numberOfBookedSeats, status);
        }
    }
}
