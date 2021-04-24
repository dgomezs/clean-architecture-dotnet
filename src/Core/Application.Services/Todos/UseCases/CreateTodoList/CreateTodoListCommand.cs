using Ardalis.GuardClauses;
using Domain.Todos.ValueObjects;
using Domain.Users.ValueObjects;

namespace Application.Services.Todos.UseCases.CreateTodoList
{
    public class CreateTodoListCommand
    {
        private CreateTodoListCommand(UserId ownerId, TodoListName todoListName)
        {
            OwnerId = ownerId;
            TodoListName = Guard.Against.Null(todoListName, nameof(todoListName));
        }

        public UserId OwnerId { get; }

        public TodoListName TodoListName { get; }

        public static CreateTodoListCommand Create(UserId ownerId, string name) =>
            new(ownerId, TodoListName.Create(name));
    }
}