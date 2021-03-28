using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Events
{
    public interface IDomainEventPublisher
    {
        Task PublishEvent(DomainEvent evt);
        Task PublishEvents(IEnumerable<DomainEvent> events);
    }
}