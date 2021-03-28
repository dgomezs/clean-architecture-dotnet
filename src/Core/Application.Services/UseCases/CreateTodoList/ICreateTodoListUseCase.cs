using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Services.UseCases.CreateTodoList
{
    public interface ICreateTodoListUseCase
    {
        Task<TodoListId> Invoke(CreateTodoListCommand createTodoListCommand);
    }
}