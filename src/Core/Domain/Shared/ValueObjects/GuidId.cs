using System;
using Ardalis.GuardClauses;

namespace Domain.Shared.ValueObjects
{
    public abstract record GuidId
    {
        protected GuidId(Guid value) =>
            Value = Guard.Against.Null(value, nameof(value));

        protected GuidId() => Value = Guid.NewGuid();
        public Guid Value { get; }
    }
}