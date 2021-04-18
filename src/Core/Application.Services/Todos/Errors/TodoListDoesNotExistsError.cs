using System.Collections;
using System.Collections.Generic;
using Domain.Shared.Errors;
using Domain.Todos.ValueObjects;

namespace Application.Services.Todos.Errors
{
    public record TodoListDoesNotExistsError : Error
    {
        public const string TodoListDoesNotExist = "TodoListDoesNotExist";
        private readonly TodoListId _todoListId;

        public TodoListDoesNotExistsError(TodoListId todoListId) : base(TodoListDoesNotExist,
            "Todo list does not exists") =>
            _todoListId = todoListId;

        public override IDictionary Data => new Dictionary<string, string>
        {
            {"TodoListId", _todoListId.Value.ToString()}
        };
    }
}