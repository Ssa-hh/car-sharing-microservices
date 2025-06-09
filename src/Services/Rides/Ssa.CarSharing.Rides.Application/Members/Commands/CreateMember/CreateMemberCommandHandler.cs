using Ssa.CarSharing.Common.Application.CQRS;
using Ssa.CarSharing.Common.Domain;
using Ssa.CarSharing.Rides.Domain.Members;

namespace Ssa.CarSharing.Rides.Application.Members.Commands.CreateMember;

internal class CreateMemberCommandHandler : ICommandHandler<CreateMemberCommand, Guid>
{
    private readonly IMemberRepository _memberRepository;

    public CreateMemberCommandHandler(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }
    public async Task<Result<Guid>> Handle(CreateMemberCommand command, CancellationToken cancellationToken)
    {
        if(await _memberRepository.ExistsAsync(command.Email, cancellationToken))
            return Result.Failure<Guid>(Error.Conflict("Members.Create", "Member already exists."));

        Member newMember = Member.Create(command.FirstName, command.LastName, command.Email);

        await _memberRepository.AddAsync(newMember, cancellationToken);

        return Result.Success(newMember.Id);
    }
}
    



