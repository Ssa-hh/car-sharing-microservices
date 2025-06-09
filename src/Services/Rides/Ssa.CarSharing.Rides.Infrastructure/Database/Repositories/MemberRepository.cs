using Ssa.CarSharing.Rides.Domain.Members;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ssa.CarSharing.Rides.Infrastructure.Database.Repositories;

internal class MemberRepository : IMemberRepository
{
    private readonly List<Member> _members = new List<Member>();
    public Task AddAsync(Member member, CancellationToken cancellationToken)
    {
        _members.Add(member);

        return Task.CompletedTask;
    }
    public Task<bool> ExistsAsync(string email, CancellationToken cancellationToken)
    {
        return Task.FromResult(_members.Any(m => m.Email==email));
    }
}
