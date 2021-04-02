using System;
using Domain.Events;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class TodoList : Entity
    {
        public TodoList(TodoListName name, TodoListId id)
        {
            Name = name;
            Id = id;
        }

        public TodoList(TodoListName name)
        {
            Id = new TodoListId(Guid.NewGuid());
            Name = name;
            Events.Add(new TodoListCreatedEvent(this));
        }

        public TodoListName Name { get; }

        public TodoListId Id { get; }
    }
}