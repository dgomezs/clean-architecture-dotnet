using System.Collections.Generic;
using Domain.Events;

namespace Domain.Entities
{
    public abstract class Aggregate
    {
        protected readonly List<DomainEvent> Events;

        public IEnumerable<DomainEvent> DomainEvents => Events;

        protected Aggregate() =>
            Events = new List<DomainEvent>();
    }
}