using Ardalis.GuardClauses;
using Domain.ValueObjects;

namespace Application.Services.UseCases.CreateTodoList
{
    public class CreateTodoListCommand
    {
        private CreateTodoListCommand(TodoListName todoListName) =>
            TodoListName = Guard.Against.Null(todoListName, nameof(todoListName));

        public TodoListName TodoListName { get; }

        public static CreateTodoListCommand Create(string name1) =>
            new(TodoListName.Create(name1));
    }
}