namespace Ssa.CarSharing.Common.Application.Authentication;

public interface IUserContext
{
    string UserEmail { get; }

    string IdentityId { get; }
}
