using Ardalis.GuardClauses;
using Domain.Todos.ValueObjects;

namespace Application.Services.UseCases.CreateTodoList
{
    public class CreateTodoListCommand
    {
        private CreateTodoListCommand(TodoListName todoListName) =>
            TodoListName = Guard.Against.Null(todoListName, nameof(todoListName));

        public TodoListName TodoListName { get; }

        public static CreateTodoListCommand Create(string name) =>
            new(TodoListName.Create(name));
    }
}