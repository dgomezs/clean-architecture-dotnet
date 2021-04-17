using System.Collections.Generic;
using Domain.Shared.Events;

namespace Domain.Shared.Entities
{
    public abstract class Aggregate
    {
        protected readonly List<DomainEvent> Events;

        public IEnumerable<DomainEvent> DomainEvents => Events;

        protected Aggregate() =>
            Events = new List<DomainEvent>();
    }
}