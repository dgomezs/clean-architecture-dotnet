using Domain.Todos.ValueObjects;

namespace Application.Services.Todos.UseCases.MarkTodoAsDone
{
    public record MarkTodoDoneCommand(TodoId TodoId)
    {
    }
}