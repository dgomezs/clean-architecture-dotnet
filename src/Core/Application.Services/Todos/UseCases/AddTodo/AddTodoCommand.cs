using Domain.Todos.ValueObjects;

namespace Application.Services.Todos.UseCases.AddTodo
{
    public record AddTodoCommand (TodoListId TodoListId, TodoDescription TodoDescription)
    {
    }
}