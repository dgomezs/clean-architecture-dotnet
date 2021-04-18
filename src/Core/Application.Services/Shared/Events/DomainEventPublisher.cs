using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Shared.Events;

namespace Application.Services.Shared.Events
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