using System;
using System.Collections;
using System.Collections.Generic;
using Domain.Shared.Errors;
using Domain.Todos.ValueObjects;

namespace Application.Services.Todos.Errors
{
    public record TodoListDoesNotExistsError : EntityNotFoundError
    {
        public const string TodoListDoesNotExist = "TodoListDoesNotExist";
        private readonly TodoListId? _todoListId;
        private readonly TodoId? _todoId;

        public TodoListDoesNotExistsError(TodoListId todoListId) : base(TodoListDoesNotExist,
            "Todo list does not exists") =>
            (_todoListId, _todoId) = (todoListId, null);


        public TodoListDoesNotExistsError(TodoId todo) : base(TodoListDoesNotExist,
            "Todo list does not exists") =>
            (_todoListId, _todoId) = (null, todo);

        public override IDictionary Data => (_todoId, _todoListId) switch
        {
            (_, null) => new Dictionary<string, string> {{"TodoId", _todoId.Value.ToString()}},
            (null, _) => new Dictionary<string, string> {{"TodoListId", _todoListId.Value.ToString()}},
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}