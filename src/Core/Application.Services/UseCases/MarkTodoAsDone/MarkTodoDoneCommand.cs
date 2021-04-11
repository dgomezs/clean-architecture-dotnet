using Domain.ValueObjects;

namespace Application.Services.UseCases.MarkTodoAsDone
{
    public record MarkTodoDoneCommand(TodoId TodoId)
    {
    }
}