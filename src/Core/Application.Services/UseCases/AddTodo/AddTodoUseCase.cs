using System.Threading.Tasks;
using Application.Services.Errors.TodoList;
using Domain.ValueObjects;

namespace Application.Services.UseCases.AddTodo
{
    public class AddTodoUseCase : IAddTodoUseCase
    {
        public Task<TodoId> AddTodo(AddTodoCommand addTodoCommand)
        {
            throw new TodoListDoesNotExistsException(addTodoCommand.TodoListId);
        }
    }
}