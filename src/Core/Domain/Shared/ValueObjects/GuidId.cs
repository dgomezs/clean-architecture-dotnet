using System;
using Ardalis.GuardClauses;

namespace Domain.Shared.ValueObjects
{
    public record GuidId
    {
        public GuidId(Guid value) =>
            Value = Guard.Against.Null(value, nameof(value));

        public Guid Value { get; }
    }
}