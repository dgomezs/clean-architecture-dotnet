using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services.Tests.TestDoubles;
using Application.Services.Todos.UseCases.CreateTodoList;
using Application.Services.Users.Errors;
using Application.Services.Users.UseCases.CreateUser;
using Autofac.Extras.Moq;
using Domain.Shared.Errors;
using Domain.Shared.Events;
using Domain.Todos.Entities;
using Domain.Todos.Events;
using Domain.Todos.ValueObjects;
using Domain.Users.ValueObjects;
using Xunit;
using static Application.Services.Shared.Extensions.EitherExtensions;

namespace Application.Services.Tests.Todos.CreateTodoList
{
    public class CreateTodoListTest
    {
        private readonly ICreateTodoListUseCase _createTodoListUseCase;
        private readonly InMemoryEventPublisher _eventPublisher;
        private readonly AutoMock _mock;
        private readonly InMemoryTodoListRepository _todoListRepository;
        private readonly ICreateUserUseCase _createUserUseCase;

        public CreateTodoListTest()
        {
            _mock = DiConfig.GetMock();
            _todoListRepository = _mock.Create<InMemoryTodoListRepository>();
            _eventPublisher = _mock.Create<InMemoryEventPublisher>();
            _createTodoListUseCase = _mock.Create<ICreateTodoListUseCase>();
            _createUserUseCase = _mock.Create<ICreateUserUseCase>();
        }

        [Fact]
        public async Task Should_create_new_todolist_when_the_list_does_not_exist()
        {
            // arrange
            var ownerId = await CreateUser();
            var createTodoListRequest = FakeCommandGenerator.FakeCreateTodoListCommand(ownerId);
            var todoListName = createTodoListRequest.TodoListName;
            await ArrangeTodoListDoesNotExist(ownerId, todoListName);
            // act
            var id = await CreateTodoListWithError(createTodoListRequest);
            // assert
            var todoList = await GetById(id);
            Assert.Equal(todoListName, todoList.Name);
            Assert.Equal(id, todoList.Id);
            Assert.Equal(ownerId, todoList.OwnerId);
        }

        [Fact]
        public async Task Should_publish_created_todo_list_event_when_a_new_todo_list_is_created()
        {
            // arrange
            var ownerId = await CreateUser();
            var createTodoListRequest = FakeCommandGenerator.FakeCreateTodoListCommand(ownerId);
            var todoListName = createTodoListRequest.TodoListName;
            await ArrangeTodoListDoesNotExist(ownerId, todoListName);
            // act
            var id = await CreateTodoList(createTodoListRequest);
            // assert
            IEnumerable<DomainEvent> domainEvents = _eventPublisher.Events;
            Assert.Single(domainEvents);
            var eDomainEvent = domainEvents.Single();
            Assert.True(eDomainEvent is TodoListCreatedEvent);
            var todoListCreatedEvent = (TodoListCreatedEvent) eDomainEvent;
            Assert.Equal(todoListName, todoListCreatedEvent.TodoList.Name);
        }

        [Fact]
        public async Task Should_not_create_todo_list_when_user_does_not_exist()
        {
            // arrange
            var ownerId = new UserId();
            var createTodoListRequest = FakeCommandGenerator.FakeCreateTodoListCommand(ownerId);
            // act
            var exception = await Assert.ThrowsAsync<DomainException>(() =>
                _createTodoListUseCase.Invoke(createTodoListRequest));
            
            var exceptionData = exception.Data;
            Assert.Equal(UserDoesNotExistError.UserDoesNotExists, exception.ErrorKey);
            Assert.Equal(createTodoListRequest.OwnerId.Value.ToString(), exceptionData["UserId"]);
        }


        [Fact]
        public async Task Should_not_create_new_todolist_when_one_by_same_name_exists()
        {
            // arrange
            var ownerId = await CreateUser();
            var createTodoListRequest = FakeCommandGenerator.FakeCreateTodoListCommand(ownerId);
            await ArrangeTodoListDoesNotExist(ownerId, createTodoListRequest.TodoListName);
            await _createTodoListUseCase.Invoke(createTodoListRequest);

            // act
            // create another list by the same
            var exception = await Assert.ThrowsAsync<DomainException>(() =>
                _createTodoListUseCase.Invoke(createTodoListRequest));
            var exceptionData = exception.Data;
            Assert.Equal(createTodoListRequest.TodoListName.Name, exceptionData["TodoListName"]);
        }

        private Task<TodoListId> CreateTodoList(CreateTodoListCommand createTodoListRequest)
        {
            return _createTodoListUseCase.Invoke(createTodoListRequest);
        }

        private async Task<TodoListId> CreateTodoListWithError(CreateTodoListCommand createTodoListRequest)
        {
            var result = await _createTodoListUseCase.InvokeWithErrors(createTodoListRequest);
            return result.ToThrowException();
        }

        private async Task ArrangeTodoListDoesNotExist(UserId ownerId, TodoListName todoListName)
        {
            await _todoListRepository.RemoveByName(ownerId, todoListName);
        }

        private Task<UserId> CreateUser()
        {
            return _createUserUseCase.Invoke(FakeCommandGenerator.FakeCreateUserCommand());
        }

        private async Task<TodoList> GetById(TodoListId id) =>
            await _todoListRepository.GetById(id) ?? throw new Exception();
    }
}