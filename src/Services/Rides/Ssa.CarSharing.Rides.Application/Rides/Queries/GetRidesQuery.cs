using Ssa.CarSharing.Common.Application.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ssa.CarSharing.Rides.Application.Rides.Queries;

public record GetRidesQuery(DateOnly? StartDate, string PickupCity, string DropOffCity) :IQuery<List<RideDto>>;
