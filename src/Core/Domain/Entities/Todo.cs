using System;
using Ardalis.GuardClauses;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Todo : Entity
    {
        public Todo(TodoDescription description)
        {
            Id = new TodoId(Guid.NewGuid());
            Description = Guard.Against.Null(description, nameof(description));
        }

        public Todo(TodoId id, TodoDescription description)
        {
            Id = Guard.Against.Null(id, nameof(id));
            Description = Guard.Against.Null(description, nameof(description));
        }

        public TodoId Id { get; }
        public TodoDescription Description { get; }
    }
}