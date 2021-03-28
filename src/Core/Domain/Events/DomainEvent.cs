using System;
using NodaTime;

namespace Domain.Events
{
    public abstract record DomainEvent
    {
        public DomainEvent()
        {
            EventId = Guid.NewGuid();
            CreatedAt = SystemClock.Instance.GetCurrentInstant();
        }

        public Guid EventId { get; }
        public Instant CreatedAt { get; }
    }
}