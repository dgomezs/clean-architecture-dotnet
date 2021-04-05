using System.Threading.Tasks;
using Application.Services.Errors.TodoList;
using Application.Services.Events;
using Application.Services.Extensions;
using Application.Services.Repositories;
using Domain.Entities;
using Domain.Errors;
using Domain.ValueObjects;
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
            return await InvokeWithErrors(createTodoListCommand).ToThrowException();
        }

        public async Task<Either<Error, TodoListId>> InvokeWithErrors(
            CreateTodoListCommand createTodoListCommand)
        {
            var todoListName = createTodoListCommand.TodoListName;
            var todoList = await _todoListRepository.GetByName(todoListName);
            if (todoList is not null)
            {
                return new TodoListAlreadyExistsError(todoListName);
            }

            var result = new TodoList(todoListName);
            await _todoListRepository.Save(result);

            await _domainEventPublisher.PublishEvents(result.DomainEvents);
            return result.Id;
        }
    }
}