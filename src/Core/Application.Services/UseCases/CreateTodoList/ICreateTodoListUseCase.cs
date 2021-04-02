using System.Threading.Tasks;
using Domain.Entities;
using Domain.Errors;
using LanguageExt;

namespace Application.Services.UseCases.CreateTodoList
{
    public interface ICreateTodoListUseCase
    {
        Task<TodoListId> Invoke(CreateTodoListCommand createTodoListCommand);
        Task<Either<Error, TodoListId>> InvokeWithErrors(CreateTodoListCommand createTodoListCommand);
    }
}