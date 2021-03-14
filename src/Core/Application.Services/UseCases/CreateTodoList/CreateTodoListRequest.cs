using Domain.ValueObjects;

namespace Application.Services.UseCases.CreateTodoList
{
    public class CreateTodoListRequest
    {
        public TodoListName TodoListName { get; }

        private CreateTodoListRequest(TodoListName todoListName) =>
            TodoListName = todoListName;

        public static CreateTodoListRequest Create(TodoListName todoListName)
        {
            return new CreateTodoListRequest(todoListName);
        }
    }
}