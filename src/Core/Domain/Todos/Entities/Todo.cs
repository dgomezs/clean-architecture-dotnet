using Ardalis.GuardClauses;
using Domain.Shared.Entities;
using Domain.Todos.ValueObjects;

namespace Domain.Todos.Entities
{
    public class Todo : Entity
    {
        public Todo(TodoDescription description)
        {
            Id = new TodoId();
            Description = Guard.Against.Null(description, nameof(description));
            Done = false;
        }

        public Todo(TodoId id, TodoDescription description, bool done)
        {
            Id = Guard.Against.Null(id, nameof(id));
            Description = Guard.Against.Null(description, nameof(description));
            Done = done;
        }

        public bool Done { get; private set; }

        public TodoId Id { get; }
        public TodoDescription Description { get; }

        public void MarkAsDone() =>
            Done = true;
    }
}