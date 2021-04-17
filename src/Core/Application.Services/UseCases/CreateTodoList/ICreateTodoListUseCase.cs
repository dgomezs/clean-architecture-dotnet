using System.Threading.Tasks;
using Domain.Shared.Errors;
using Domain.Todos.ValueObjects;
using LanguageExt;

namespace Application.Services.UseCases.CreateTodoList
{
    public interface ICreateTodoListUseCase
    {
        Task<TodoListId> Invoke(CreateTodoListCommand createTodoListCommand);
        Task<Either<Error, TodoListId>> InvokeWithErrors(CreateTodoListCommand createTodoListCommand);
    }
}