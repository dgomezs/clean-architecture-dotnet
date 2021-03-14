using System.Threading.Tasks;
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

        public async Task<TodoListId> Invoke(TodoListName todoListName)
        {
            var todoList = await TodoListRepository.GetByName(todoListName) ?? new TodoList(todoListName);
            return await TodoListRepository.Save(todoList);
        }
    }
}