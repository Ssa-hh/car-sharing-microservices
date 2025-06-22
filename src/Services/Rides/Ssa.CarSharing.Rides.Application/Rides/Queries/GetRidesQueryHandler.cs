using Ssa.CarSharing.Common.Application.CQRS;
using Ssa.CarSharing.Common.Domain;
using Ssa.CarSharing.Rides.Domain.Rides;

namespace Ssa.CarSharing.Rides.Application.Rides.Queries;

internal class GetRidesQueryHandler : IQueryHandler<GetRidesQuery, List<RideDto>>
{
    private readonly IRideRepository _rideRepository;

    public GetRidesQueryHandler(IRideRepository rideRepository)
    {
        _rideRepository = rideRepository;
    }

    public async Task<Result<List<RideDto>>> Handle(GetRidesQuery query, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(query.PickupCity) || string.IsNullOrWhiteSpace(query.DropOffCity))
            return Result.Failure<List<RideDto>>(Error.Failure("", "Pickup and drop off cities are required."));

        DateOnly startDate = (!query.RideDate.HasValue || query.RideDate < DateOnly.FromDateTime(DateTime.Today) )? DateOnly.FromDateTime(DateTime.Today) : query.RideDate.Value;


        var rides = await _rideRepository.FindAsync(startDate, query.PickupCity, query.DropOffCity, true, cancellationToken);

        return Result.Success(rides.Select(r => RideDto.FromRide(r)).ToList());
    }
}
