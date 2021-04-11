using System.Threading.Tasks;
using Domain.ValueObjects;

namespace Application.Services.UseCases.AddTodo
{
    public interface IAddTodoUseCase
    {
        Task<TodoId> Invoke(AddTodoCommand addTodoCommand);
    }
}