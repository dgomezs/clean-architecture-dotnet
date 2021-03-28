using System.Collections;
using System.Collections.Generic;
using Domain.ValueObjects;

namespace Application.Services.Errors
{
    public class TodoListAlreadyExistsException : EntityExistsException
    {
        private readonly TodoListName _todoListName;

        public TodoListAlreadyExistsException(TodoListName todoListName) : base(ErrorCodes.TodoListAlreadyExists,
            $"Todo list already exists") =>
            _todoListName = todoListName;

        public override IDictionary Data
        {
            get
            {
                return new Dictionary<string, string>
                {
                    {"TodoListName", _todoListName.Name}
                };
            }
        }
    }
}