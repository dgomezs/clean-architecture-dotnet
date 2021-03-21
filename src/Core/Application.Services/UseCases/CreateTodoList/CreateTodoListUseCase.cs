using System.Threading.Tasks;
using Application.Services.Errors;
using Application.Services.Repositories;
using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Services.UseCases.CreateTodoList
{
    public class CreateTodoListUseCase : ICreateTodoListUseCase
    {
        public CreateTodoListUseCase(ITodoListRepository todoListRepository) =>
            TodoListRepository = todoListRepository;

        private ITodoListRepository TodoListRepository { get; }

        public async Task<long> Invoke(CreateTodoListCommand createTodoListCommand)
        {
            var todoListName = createTodoListCommand.TodoListName;
            var todoList = await TodoListRepository.GetByName(todoListName);
            if (todoList is not null) throw new TodoListAlreadyExistsException(todoListName);

            return await TodoListRepository.Save(new TodoList(todoListName));
        }
    }
}