namespace Application.Services.Errors.TodoList
{
    public record TodoListAlreadyExistsError : EntityAlreadyExistsError
    {
        public const string TodoListAlreadyExists = "TodoListAlreadyExists";

        public TodoListAlreadyExistsError() : base(TodoListAlreadyExists,
            "Todo list already exists")
        {
        }
    }
}