using System.Threading.Tasks;
using Application.Services.Shared.Events;
using Application.Services.Todos.Errors;
using Application.Services.Todos.Repositories;
using Application.Services.Users.Errors;
using Application.Services.Users.Repositories;
using Domain.Shared.Errors;
using Domain.Todos.ValueObjects;

namespace Application.Services.Todos.UseCases.AddTodo
{
    public class AddTodoUseCase : IAddTodoUseCase
    {
        private readonly IDomainEventPublisher _domainEventPublisher;
        private readonly ITodoListRepository _todoListRepository;
        private readonly IUserRepository _userRepository;

        public AddTodoUseCase(ITodoListRepository todoListRepository, IDomainEventPublisher domainEventPublisher,
            IUserRepository userRepository)
        {
            _todoListRepository = todoListRepository;
            _domainEventPublisher = domainEventPublisher;
            _userRepository = userRepository;
        }

        public async Task<TodoId> Invoke(AddTodoCommand addTodoCommand)
        {
            var owner = await _userRepository.GetById(addTodoCommand.OwnerId) ?? throw new DomainException(
                new UserDoesNotExistError(addTodoCommand.OwnerId));

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