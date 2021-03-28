using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Events;

namespace Application.Services.Tests.TestDoubles
{
    public class InMemoryEventPublisher : IDomainEventPublisher
    {
        public readonly List<DomainEvent> Events = new();

        public Task PublishEvent(DomainEvent evt)
        {
            Events.Add(evt);
            return Task.CompletedTask;
        }

        public Task PublishEvents(IEnumerable<DomainEvent> events)
        {
            Events.AddRange(events);
            return Task.CompletedTask;
        }
    }
}