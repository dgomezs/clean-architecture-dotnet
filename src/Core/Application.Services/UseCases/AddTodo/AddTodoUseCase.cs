using System.Threading.Tasks;
using Application.Services.Errors.TodoList;
using Application.Services.Repositories;
using Domain.Errors;
using Domain.ValueObjects;

namespace Application.Services.UseCases.AddTodo
{
    public class AddTodoUseCase : IAddTodoUseCase
    {
        private readonly ITodoListRepository _todoListRepository;

        public AddTodoUseCase(ITodoListRepository todoListRepository) =>
            _todoListRepository = todoListRepository;

        public async Task<TodoId> AddTodo(AddTodoCommand addTodoCommand)
        {
            var todoList = await _todoListRepository.GetById(addTodoCommand.TodoListId)
                           ?? throw new DomainException(
                               new TodoListDoesNotExistsError(addTodoCommand.TodoListId));

            return todoList.AddTodo(addTodoCommand.TodoDescription);
        }
    }
}