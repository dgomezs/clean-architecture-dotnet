using System.Collections;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Errors;

namespace Application.Services.Errors.TodoList
{
    public class TodoListDoesNotExistsException : DomainException
    {
        private readonly TodoListId _todoListId;

        public TodoListDoesNotExistsException(TodoListId todoListId) : base(new TodoListDoesNotExistsError()) =>
            _todoListId = todoListId;

        public override IDictionary Data => new Dictionary<string, string>
        {
            {"TodoListId", _todoListId.Value.ToString()}
        };
    }
}