using System;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class TodoList
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
        }

        public TodoListName Name { get; }

        //TODO make this own id
        // TODO add events
        public TodoListId Id { get; }
    }
}