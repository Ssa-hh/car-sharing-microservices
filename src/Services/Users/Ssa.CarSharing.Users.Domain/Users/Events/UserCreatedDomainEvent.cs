using Ssa.CarSharing.Common.Domain;

namespace Ssa.CarSharing.Users.Domain.Users.Events
{
    internal class UserCreatedDomainEvent : DomainEvent
    {
        public UserCreatedDomainEvent(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; private set; }
    }
}