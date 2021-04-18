using System.Threading.Tasks;

namespace Application.Services.Todos.UseCases.MarkTodoAsDone
{
    public interface IMarkTodoDoneUseCase
    {
        Task Invoke(MarkTodoDoneCommand markTodoDoneCommand);
    }
}