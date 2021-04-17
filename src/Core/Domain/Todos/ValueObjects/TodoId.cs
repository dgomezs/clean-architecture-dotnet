using System;
using Domain.Shared.ValueObjects;

namespace Domain.Todos.ValueObjects
{
    public record TodoId : GuidId
    {
        public TodoId(Guid value) : base(value)
        {
        }
    }
}