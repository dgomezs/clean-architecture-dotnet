using Domain.ValueObjects;

namespace Domain.Entities
{
    public class TodoList
    {
        public TodoList(TodoListName name, long id)
        {
            Name = name;
            Id = id;
        }

        public TodoList(TodoListName name) =>
            Name = name;

        public TodoListName Name { get; }
        public long? Id { get; }
    }
}