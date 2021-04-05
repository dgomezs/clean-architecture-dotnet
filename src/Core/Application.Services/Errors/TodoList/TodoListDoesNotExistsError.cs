using System.Collections;
using System.Collections.Generic;
using Domain.Errors;
using Domain.ValueObjects;

namespace Application.Services.Errors.TodoList
{
    public record TodoListDoesNotExistsError : Error
    {
        public const string TodoListDoesNotExist = "TodoListDoesNotExist";
        private readonly TodoListId _todoListId;

        public TodoListDoesNotExistsError(TodoListId todoListId) : base(TodoListDoesNotExist,
            "Todo list does not exists")
        {
            _todoListId = todoListId;
        }
        
        public override IDictionary Data => new Dictionary<string, string>
        {
            {"TodoListId", _todoListId.Value.ToString()}
        };
    }
}