using System.Threading.Tasks;
using Application.Services.Shared.Events;
using Application.Services.Shared.Extensions;
using Application.Services.Todos.Errors;
using Application.Services.Todos.Repositories;
using Application.Services.Users.Repositories;
using Domain.Shared.Errors;
using Domain.Todos.Entities;
using Domain.Todos.ValueObjects;
using LanguageExt;

namespace Application.Services.Todos.UseCases.CreateTodoList
{
    public class CreateTodoListUseCase : ICreateTodoListUseCase
    {
        private readonly IDomainEventPublisher _domainEventPublisher;

        private readonly ITodoListRepository _todoListRepository;
        private readonly IUserRepository _userRepository;

        public CreateTodoListUseCase(ITodoListRepository todoListRepository,
            IDomainEventPublisher domainEventPublisher, IUserRepository userRepository)
        {
            _todoListRepository = todoListRepository;
            _domainEventPublisher = domainEventPublisher;
            _userRepository = userRepository;
        }

        public async Task<TodoListId> Invoke(CreateTodoListCommand createTodoListCommand) =>
            await InvokeWithErrors(createTodoListCommand).ToThrowException();

        public async Task<Either<Error, TodoListId>> InvokeWithErrors(
            CreateTodoListCommand createTodoListCommand)
        {
            var existingOwner = await _userRepository.GetById(createTodoListCommand.OwnerId);
            var todoListName = createTodoListCommand.TodoListName;
            if (existingOwner is null)
                return new TodoListAlreadyExistsError(todoListName);
            var todoList = await _todoListRepository.GetByName(createTodoListCommand.OwnerId, todoListName);
            if (todoList is not null)
                return new TodoListAlreadyExistsError(todoListName);

            var result = new TodoList(createTodoListCommand.OwnerId, todoListName);
            await _todoListRepository.Save(result);

            await _domainEventPublisher.PublishEvents(result.DomainEvents);
            return result.Id;
        }
    }
}