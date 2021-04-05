using System.Threading.Tasks;
using Application.Services.Errors.TodoList;
using Application.Services.Events;
using Application.Services.Repositories;
using Domain.Errors;
using Domain.ValueObjects;

namespace Application.Services.UseCases.AddTodo
{
    public class AddTodoUseCase : IAddTodoUseCase
    {
        private readonly ITodoListRepository _todoListRepository;
        private readonly IDomainEventPublisher _domainEventPublisher;

        public AddTodoUseCase(ITodoListRepository todoListRepository, IDomainEventPublisher domainEventPublisher)
        {
            _todoListRepository = todoListRepository;
            _domainEventPublisher = domainEventPublisher;
        }

        public async Task<TodoId> AddTodo(AddTodoCommand addTodoCommand)
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