using Domain.Shared.Events;
using Domain.Todos.Entities;

namespace Domain.Todos.Events
{
    public record TodoListCreatedEvent (TodoList TodoList) : DomainEvent
    {
    }
}