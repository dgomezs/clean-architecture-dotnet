using System.Threading.Tasks;
using Domain.ValueObjects;

namespace Application.Services.UseCases.CreateTodoList
{
    public interface ICreateTodoListUseCase
    {
        Task<TodoListId> Invoke(TodoListName todoListName);
    }
}