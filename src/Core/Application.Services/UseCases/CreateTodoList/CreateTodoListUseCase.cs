using System.Threading.Tasks;
using Application.Services.Errors;
using Application.Services.Repositories;
using Domain.Entities;

namespace Application.Services.UseCases.CreateTodoList
{
    public class CreateTodoListUseCase : ICreateTodoListUseCase
    {
        public CreateTodoListUseCase(ITodoListRepository todoListRepository) =>
            TodoListRepository = todoListRepository;

        private ITodoListRepository TodoListRepository { get; }

        public async Task<TodoListId> Invoke(CreateTodoListCommand createTodoListCommand)
        {
            var todoListName = createTodoListCommand.TodoListName;
            var todoList = await TodoListRepository.GetByName(todoListName);
            if (todoList is not null) throw new TodoListAlreadyExistsException(todoListName);

            var result = new TodoList(todoListName);
            await TodoListRepository.Save(result);
            return result.Id;
        }
    }
}