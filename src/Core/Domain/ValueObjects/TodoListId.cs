using System;
using Ardalis.GuardClauses;

namespace Domain.ValueObjects
{
    public record TodoListId
    {
        public TodoListId(Guid value) =>
            Value = Guard.Against.Null(value, nameof(value));

        public Guid Value { get; }
    }
}