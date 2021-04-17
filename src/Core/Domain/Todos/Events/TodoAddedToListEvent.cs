using Domain.Shared.Events;
using Domain.Todos.Entities;
using Domain.Todos.ValueObjects;

namespace Domain.Todos.Events
{
    public record TodoAddedToListEvent(TodoListId TodoListId, Todo Todo) : DomainEvent
    {
    }
}