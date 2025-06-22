using System.Linq.Expressions;

namespace Ssa.CarSharing.Rides.Domain.Rides;

public interface IRideRepository
{
    Task<Ride?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task AddAsync(Ride ride, CancellationToken cancellationToken = default);

    Task<long> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<long> ReplaceAsync(Ride ride, CancellationToken cancellationToken = default);

    Task<IEnumerable<Ride>> FindAsync(DateOnly? rideDate, string? pickupCity, string? dropOffCity, bool onlyOpenOrComplete = true, CancellationToken cancellationToken = default);
}
