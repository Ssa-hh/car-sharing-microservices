namespace Ssa.CarSharing.Rides.Domain.Rides;

public interface IRideRepository
{
    Task<Ride?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task AddAsync(Ride ride, CancellationToken cancellationToken = default);

    Task<long> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<long> ReplaceAsync(Ride ride, CancellationToken cancellationToken = default);

}
