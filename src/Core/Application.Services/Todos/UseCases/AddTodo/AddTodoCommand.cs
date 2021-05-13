using Domain.Todos.ValueObjects;
using Domain.Users.ValueObjects;

namespace Application.Services.Todos.UseCases.AddTodo
{
    public record AddTodoCommand (UserId OwnerId, TodoListId TodoListId, TodoDescription TodoDescription)
    {
    }
}