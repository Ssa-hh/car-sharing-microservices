using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ssa.CarSharing.Common.Domain
{
    public class Entity
    {
        private readonly List<IDomainEvent> _domainEvents = [];

        public Entity(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; init; }
        
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
        
        public void AddDomainEvent(IDomainEvent domainEvent)
        {
            if (domainEvent == null) throw new ArgumentNullException(nameof(domainEvent));
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
