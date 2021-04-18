using System.Collections;
using System.Collections.Generic;
using Domain.Shared.Errors;
using Domain.Todos.ValueObjects;

namespace Application.Services.Todos.Errors
{
    public record TodoListWithTodoNotFound : Error
    {
        public const string TodoListWithTodoDoesNotExist = "TodoListWithTodoNotFound";
        private readonly TodoId _todoId;

        public TodoListWithTodoNotFound(TodoId todoId) : base(TodoListWithTodoDoesNotExist,
            "Todo list does not exists") =>
            _todoId = todoId;

        public override IDictionary Data => new Dictionary<string, string>
        {
            {"TodoId", _todoId.Value.ToString()}
        };
    }
}