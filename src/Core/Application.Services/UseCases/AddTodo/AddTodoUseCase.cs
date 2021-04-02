using System;
using System.Threading.Tasks;
using Application.Services.Errors.TodoList;
using Application.Services.Repositories;
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
                           ?? throw new TodoListDoesNotExistsException(addTodoCommand.TodoListId);
            return new TodoId(Guid.NewGuid());
        }
    }
}