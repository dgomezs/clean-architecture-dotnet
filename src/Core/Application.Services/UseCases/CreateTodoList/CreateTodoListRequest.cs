using Ardalis.GuardClauses;
using Domain.ValueObjects;

namespace Application.Services.UseCases.CreateTodoList
{
    public class CreateTodoListRequest
    {
        public TodoListName TodoListName { get; }

        public CreateTodoListRequest(TodoListName todoListName) =>
            TodoListName = Guard.Against.Null(todoListName, "todoListName can not be null");
    }
}