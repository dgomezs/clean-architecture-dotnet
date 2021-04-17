using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Shared.Events;

namespace Application.Services.Events
{
    public interface IDomainEventPublisher
    {
        Task PublishEvent(DomainEvent evt);
        Task PublishEvents(IEnumerable<DomainEvent> events);
    }
}