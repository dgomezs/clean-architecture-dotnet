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

        public TodoList(TodoListName todoListName) =>
            Name = todoListName;

        public TodoListName Name { get; }
        public TodoListId? Id { get; }
    }
}