using System.Threading.Tasks;
using Application.Services.Shared.Events;
using Application.Services.Shared.Extensions;
using Application.Services.Todos.Errors;
using Application.Services.Todos.Repositories;
using Application.Services.Users.Errors;
using Application.Services.Users.Repositories;
using Domain.Shared.Errors;
using Domain.Todos.Entities;
using Domain.Todos.ValueObjects;
using Domain.Users.ValueObjects;
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
            return await CheckExistingOwnerExists(createTodoListCommand.OwnerId)
                .ToAsync()
                .Bind(u => CreateTodoList(createTodoListCommand)
                    .ToAsync())
                .MapAsync(PublishEvents)
                .Map(todoList => todoList.Id);
        }

        private async Task<Either<Error, Unit>> CheckExistingOwnerExists(UserId ownerId)
        {
            var existingOwner = await _userRepository.GetById(ownerId);
            if (existingOwner is null)
                return new UserDoesNotExistError(ownerId);
            return Unit.Default;
        }

        private async Task<Either<Error, TodoList>> CreateTodoList(CreateTodoListCommand createTodoListCommand)
        {
            var todoListName = createTodoListCommand.TodoListName;
            var todoList = await _todoListRepository.GetByName(createTodoListCommand.OwnerId, todoListName);
            if (todoList is not null)
                return new TodoListAlreadyExistsError(todoListName);

            var newTodoList = new TodoList(createTodoListCommand.OwnerId, todoListName);
            await _todoListRepository.Save(newTodoList);
            return newTodoList;
        }

        private async Task<TodoList> PublishEvents(TodoList todoList)
        {
            await _domainEventPublisher.PublishEvents(todoList.DomainEvents);
            return todoList;
        }
    }
}