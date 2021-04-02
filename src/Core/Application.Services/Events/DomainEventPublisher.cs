using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Events;

namespace Application.Services.Events
{
    public class DomainEventPublisher : IDomainEventPublisher
    {
        public Task PublishEvent(DomainEvent evt)
        {
            return Task.CompletedTask;
        }

        public Task PublishEvents(IEnumerable<DomainEvent> events)
        {
            return Task.CompletedTask;
        }
    }
}