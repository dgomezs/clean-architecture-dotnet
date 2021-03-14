using System.Threading.Tasks;
using Application.Services.Tests.CreateTodoList;
using Domain.ValueObjects;

namespace Application.Services.UseCases.CreateTodoList
{
    public interface ICreateTodoListUseCase
    {
        Task<TodoListId> Invoke(CreateTodoListRequest createTodoListRequest);
    }
}