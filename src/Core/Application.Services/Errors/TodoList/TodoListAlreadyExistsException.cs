using System.Collections;
using System.Collections.Generic;
using Domain.Errors;
using Domain.ValueObjects;

namespace Application.Services.Errors.TodoList
{
    public class TodoListAlreadyExistsException : DomainException
    {
        private readonly TodoListName _todoListName;

        public TodoListAlreadyExistsException(TodoListName todoListName) :
            base(new TodoListAlreadyExistsError()) =>
            _todoListName = todoListName;

        public override IDictionary Data => new Dictionary<string, string>
        {
            {"TodoListName", _todoListName.Name}
        };
    }
}