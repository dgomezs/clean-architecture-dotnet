using System.Collections;
using System.Collections.Generic;
using Domain.ValueObjects;

namespace Application.Services.Errors.TodoList
{
    public record TodoListAlreadyExistsError : EntityAlreadyExistsError
    {
        public const string TodoListAlreadyExists = "TodoListAlreadyExists";
        private readonly TodoListName _todoListName;

        public TodoListAlreadyExistsError(TodoListName todoListName) : base(TodoListAlreadyExists,
            "Todo list already exists") =>
            _todoListName = todoListName;

        public override IDictionary Data => new Dictionary<string, string>
        {
            {"TodoListName", _todoListName.Name}
        };
    }
}