﻿using System.Collections;
using System.Collections.Generic;
using Application.Services.Shared.Errors;
using Domain.Todos.ValueObjects;

namespace Application.Services.Todos.Errors
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