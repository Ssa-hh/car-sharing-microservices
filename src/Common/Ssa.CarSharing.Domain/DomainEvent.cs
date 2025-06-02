using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ssa.CarSharing.Common.Domain
{
    public class DomainEvent: IDomainEvent
    {
        protected DomainEvent()
        {
            Id = Guid.NewGuid();
            OccurredOnUtc = DateTime.UtcNow;
        }

        public Guid Id { get; private set; }
        public DateTime OccurredOnUtc { get; private set; }
    }
}
