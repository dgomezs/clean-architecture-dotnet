using Domain.Todos.ValueObjects;

namespace Application.Services.UseCases.AddTodo
{
    public record AddTodoCommand (TodoListId TodoListId, TodoDescription TodoDescription)
    {
    }
}