using Ssa.CarSharing.Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ssa.CarSharing.Users.Domain.Users.Events
{
    public class CarAddedToUserDomainEvent : DomainEvent
    {
        public CarAddedToUserDomainEvent(Guid userId, Guid carId)
        {
            UserId = userId;

            CarId = carId;
        }

        public Guid UserId { get; private set; }

        public Guid CarId { get; private set; }
    }
}
