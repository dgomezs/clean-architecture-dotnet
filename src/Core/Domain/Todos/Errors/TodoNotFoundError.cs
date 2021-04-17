using System.Collections;
using System.Collections.Generic;
using Domain.Shared.Errors;
using Domain.Todos.ValueObjects;

namespace Domain.Todos.Errors
{
    public record TodoNotFoundError : EntityNotFoundError
    {
        private const string TodoNotFoundErrorKey = "TodoNotFoundError";

        public TodoNotFoundError(TodoId todoId)
            : base(TodoNotFoundErrorKey, "No todo found with that id") =>
            TodoId = todoId.Value.ToString();

        private string TodoId { get; }

        public override IDictionary Data => new Dictionary<string, string>
        {
            {"TodoId", TodoId}
        };
    }
}