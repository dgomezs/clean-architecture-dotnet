using System.Collections.Generic;
using Domain.Events;

namespace Domain.Entities
{
    public abstract class Entity
    {
        protected readonly List<DomainEvent> Events;

        public IEnumerable<DomainEvent> DomainEvents => Events;

        protected Entity() =>
            Events = new List<DomainEvent>();
    }
}