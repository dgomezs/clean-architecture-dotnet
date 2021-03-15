using Ardalis.GuardClauses;
using Domain.ValueObjects;

namespace Application.Services.UseCases.CreateTodoList
{
    public class CreateTodoListRequest
    {
        private CreateTodoListRequest(TodoListName todoListName, TodoListName todoListName2)
        {
            TodoListName = Guard.Against.Null(todoListName, nameof(todoListName));
            TodoListName2 = Guard.Against.Null(todoListName2, nameof(todoListName));
        }

        public TodoListName TodoListName { get; }
        public TodoListName TodoListName2 { get; }

        public static CreateTodoListRequest Create(string name1,
            string name2)
        {
            return new CreateTodoListRequest(TodoListName.Create(name1), TodoListName.Create(name2));
        }
    }
}