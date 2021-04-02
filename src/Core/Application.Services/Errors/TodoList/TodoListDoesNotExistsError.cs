using Domain.Errors;

namespace Application.Services.Errors.TodoList
{
    public record TodoListDoesNotExistsError : Error
    {
        public const string TodoListDoesNotExist = "TodoListDoesNotExist";

        public TodoListDoesNotExistsError() : base(TodoListDoesNotExist,
            "Todo list does not exists")
        {
        }
    }
}