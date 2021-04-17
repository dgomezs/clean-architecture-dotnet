using System.Collections.Generic;
using Domain.Shared.Events;

namespace Domain.Shared.Entities
{
    public abstract class Entity
    {
        protected readonly List<DomainEvent> Events;

        protected Entity() =>
            Events = new List<DomainEvent>();

        public IEnumerable<DomainEvent> DomainEvents => Events;
    }
}