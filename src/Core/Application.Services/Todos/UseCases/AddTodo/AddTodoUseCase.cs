using System.Threading.Tasks;
using Application.Services.Shared.Events;
using Application.Services.Todos.Errors;
using Application.Services.Todos.Repositories;
using Domain.Shared.Errors;
using Domain.Todos.ValueObjects;

namespace Application.Services.Todos.UseCases.AddTodo
{
    public class AddTodoUseCase : IAddTodoUseCase
    {
        private readonly IDomainEventPublisher _domainEventPublisher;
        private readonly ITodoListRepository _todoListRepository;

        public AddTodoUseCase(ITodoListRepository todoListRepository, IDomainEventPublisher domainEventPublisher)
        {
            _todoListRepository = todoListRepository;
            _domainEventPublisher = domainEventPublisher;
        }

        public async Task<TodoId> Invoke(AddTodoCommand addTodoCommand)
        {
            var todoList = await _todoListRepository.GetById(addTodoCommand.TodoListId)
                           ?? throw new DomainException(
                               new TodoListDoesNotExistsError(addTodoCommand.TodoListId));

            var todoId = todoList.AddTodo(addTodoCommand.TodoDescription);

            await _todoListRepository.Save(todoList);
            await _domainEventPublisher.PublishEvents(todoList.DomainEvents);
            return todoId;
        }
    }
}