using Domain.Entities;

namespace Domain.Events
{
    public record TodoListCreatedEvent (TodoList TodoList) : DomainEvent
    {
    }
}