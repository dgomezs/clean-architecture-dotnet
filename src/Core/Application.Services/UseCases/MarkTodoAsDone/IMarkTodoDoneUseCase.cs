using System.Threading.Tasks;

namespace Application.Services.UseCases.MarkTodoAsDone
{
    public interface IMarkTodoDoneUseCase
    {
        Task Invoke(MarkTodoDoneCommand markTodoDoneCommand);
    }
}