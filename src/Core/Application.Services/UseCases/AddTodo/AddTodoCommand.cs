using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Services.UseCases.AddTodo
{
    public record AddTodoCommand (TodoListId TodoListId, TodoDescription TodoDescription)
    {
    }
}