using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Events
{
    public record TodoAddedToListEvent(TodoListId TodoListId, Todo Todo) : DomainEvent
    {
    }
}