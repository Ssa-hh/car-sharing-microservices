using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ssa.CarSharing.Rides.Domain.Rides;

public enum RideStatus
{
    Open = 0, // Ride is available for booking
    InProgress = 1, // Ride is currently ongoing
    Completed = 2, // Ride has been successfully completed
    Canceled = 3 // Ride was canceled
}
