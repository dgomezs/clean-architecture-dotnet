using System.Collections;
using System.Collections.Generic;
using Domain.Shared.Errors;
using Domain.Todos.ValueObjects;

namespace Application.Services.Todos.Errors
{
    public record TodoListDoesNotExistsError : EntityNotFoundError
    {
        public const string TodoListDoesNotExist = "TodoListDoesNotExist";
        private const string ErrorMessage = "Todo list does not exists";
        private readonly string _todoListId;
        private readonly string _todoId;

        public TodoListDoesNotExistsError(TodoListId todoListId) : base(TodoListDoesNotExist,
            ErrorMessage) =>
            (_todoListId, _todoId) = (todoListId.Value.ToString(), string.Empty);


        public TodoListDoesNotExistsError(TodoId todo) : base(TodoListDoesNotExist,
            ErrorMessage) =>
            (_todoListId, _todoId) = (string.Empty, todo.Value.ToString());

        public override IDictionary Data => new Dictionary<string, string>
        {
            {"TodoId", _todoId},
            {"TodoListId", _todoListId}
        };
    }
}