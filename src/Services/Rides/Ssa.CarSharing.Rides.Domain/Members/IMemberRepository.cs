namespace Ssa.CarSharing.Rides.Domain.Members;

public interface IMemberRepository
{
    // Task<Member?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Member member, CancellationToken cancellationToken = default);
    // Task UpdateAsync(Member member, CancellationToken cancellationToken = default);
    // Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string email, CancellationToken cancellationToken = default);
}
