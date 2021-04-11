using System;
using Ardalis.GuardClauses;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Todo : Entity
    {
        private bool _done;

        public Todo(TodoDescription description)
        {
            Id = new TodoId(Guid.NewGuid());
            Description = Guard.Against.Null(description, nameof(description));
            _done = false;
        }

        public Todo(TodoId id, TodoDescription description, bool done)
        {
            Id = Guard.Against.Null(id, nameof(id));
            Description = Guard.Against.Null(description, nameof(description));
            _done = done;
        }

        public TodoId Id { get; }
        public TodoDescription Description { get; }

        public bool IsDone() =>
            _done;

        public void MarkAsDone() =>
            _done = true;
    }
}