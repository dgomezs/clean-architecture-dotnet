using System.Threading.Tasks;

namespace Application.Services.UseCases.CreateTodoList
{
    public interface ICreateTodoListUseCase
    {
        Task<long> Invoke(CreateTodoListCommand createTodoListCommand);
    }
}