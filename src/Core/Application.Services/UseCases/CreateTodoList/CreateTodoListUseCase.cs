using System.Threading.Tasks;
using Application.Services.Errors;
using Application.Services.Errors.TodoList;
using Application.Services.Events;
using Application.Services.Repositories;
using Domain.Entities;
using Domain.Errors;
using LanguageExt;

namespace Application.Services.UseCases.CreateTodoList
{
    public class CreateTodoListUseCase : ICreateTodoListUseCase
    {
        public CreateTodoListUseCase(ITodoListRepository todoListRepository,
            IDomainEventPublisher domainEventPublisher)
        {
            _todoListRepository = todoListRepository;
            _domainEventPublisher = domainEventPublisher;
        }

        private readonly ITodoListRepository _todoListRepository;
        private readonly IDomainEventPublisher _domainEventPublisher;

        public async Task<TodoListId> Invoke(CreateTodoListCommand createTodoListCommand)
        {
            var todoListName = createTodoListCommand.TodoListName;
            var todoList = await _todoListRepository.GetByName(todoListName);
            if (todoList is not null) throw new TodoListAlreadyExistsException(todoListName);

            var result = new TodoList(todoListName);
            await _todoListRepository.Save(result);

            await _domainEventPublisher.PublishEvents(result.DomainEvents);
            return result.Id;
        }

        public async Task<Either<Error, TodoListId>> InvokeWithErrors(
            CreateTodoListCommand createTodoListCommand)
        {
            var todoListName = createTodoListCommand.TodoListName;
            var todoList = await _todoListRepository.GetByName(todoListName);
            if (todoList is not null)
            {
                return new TodoListAlreadyExistsError();
            }

            var result = new TodoList(todoListName);
            await _todoListRepository.Save(result);

            await _domainEventPublisher.PublishEvents(result.DomainEvents);
            return result.Id;
        }
    }
}