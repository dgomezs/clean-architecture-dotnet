using System;
using Domain.Shared.ValueObjects;

namespace Domain.Todos.ValueObjects
{
    public record TodoListId : GuidId
    {
        public TodoListId(Guid value) : base(value)
        {
        }

        public TodoListId()
        {
        }
    }
}