using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ssa.CarSharing.Common.Application.EventBus
{
    public class IntegrationEvent
    {
        public IntegrationEvent()
        {
            Id = System.Guid.NewGuid();
            OccurredOnUtc = DateTime.UtcNow;
        }
        public Guid Id { get; private set; }
        public DateTime OccurredOnUtc { get; private set; }
    }
}
