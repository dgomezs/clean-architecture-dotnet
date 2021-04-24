using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services.Tests.TestDoubles;
using Application.Services.Todos.UseCases.AddTodo;
using Application.Services.Todos.UseCases.CreateTodoList;
using Application.Services.Users.UseCases.CreateUser;
using Autofac.Extras.Moq;
using Domain.Shared.Errors;
using Domain.Shared.Events;
using Domain.Todos.Entities;
using Domain.Todos.Events;
using Domain.Todos.ValueObjects;
using Domain.Users.ValueObjects;
using FakeTestData;
using FluentAssertions;
using Xunit;

namespace Application.Services.Tests.Todos.AddTodoToList
{
    public class AddTodoToListTest
    {
        private readonly IAddTodoUseCase _addTodoUseCase;
        private readonly ICreateTodoListUseCase _createTodoListUseCase;
        private readonly InMemoryEventPublisher _eventPublisher;
        private readonly AutoMock _mock;
        private readonly InMemoryTodoListRepository _todoListRepository;
        private readonly ICreateUserUseCase _createUserUseCase;

        public AddTodoToListTest()
        {
            _mock = DiConfig.GetMock();
            _todoListRepository = _mock.Create<InMemoryTodoListRepository>();
            _eventPublisher = _mock.Create<InMemoryEventPublisher>();
            _createTodoListUseCase = _mock.Create<ICreateTodoListUseCase>();
            _addTodoUseCase = _mock.Create<IAddTodoUseCase>();
            _createUserUseCase = _mock.Create<ICreateUserUseCase>();
        }

        [Fact]
        public async Task Should_add_todo_when_list_exists_and_max_number_todos_not_reached()
        {
            // arrange
            var todoListId = await ArrangeTodoListExistWithNoTodos();
            var todoDescription = TodosFakeData.CreateTodoDescription();
            var addTodoCommand = new AddTodoCommand(todoListId, todoDescription);
            // act
            var todoId = await _addTodoUseCase.Invoke(addTodoCommand);
            // assert
            Assert.NotNull(todoId);
            var todoList = await _todoListRepository.GetById(todoListId) ?? throw new Exception();
            todoList.Todos.Should().Contain(t => todoId.Equals(t.Id));
            todoList.Todos.Should().Contain(t => todoDescription.Equals(t.Description));
        }

        [Fact]
        public async Task Should_publish_todo_added_event_when_adding_a_todo_successfully()
        {
            // arrange
            var todoListId = await ArrangeTodoListExistWithNoTodos();
            var todoDescription = TodosFakeData.CreateTodoDescription();
            var addTodoCommand = new AddTodoCommand(todoListId, todoDescription);
            _eventPublisher.ClearEvents();
            // act
            await _addTodoUseCase.Invoke(addTodoCommand);
            // assert
            IEnumerable<DomainEvent> domainEvents = _eventPublisher.Events;
            Assert.Single(domainEvents);
            var eDomainEvent = domainEvents.Single();
            Assert.True(eDomainEvent is TodoAddedToListEvent);
            var (todoListId1, todo) = (TodoAddedToListEvent) eDomainEvent;
            Assert.Equal(todoDescription, todo.Description);
            Assert.Equal(todoListId, todoListId1);
        }


        [Fact]
        public async Task Should_throw_an_error_when_todo_list_does_not_exist()
        {
            // arrange
            var todoListId = await ArrangeTodoListDoesNotExist();
            var addTodoCommand = new AddTodoCommand(todoListId, TodosFakeData.CreateTodoDescription());
            // act / assert
            var exception = await Assert.ThrowsAsync<DomainException>(() =>
                _addTodoUseCase.Invoke(addTodoCommand));
            var exceptionData = exception.Data;
            Assert.Equal(todoListId.Value.ToString(), exceptionData["TodoListId"]);
        }

        [Fact]
        public async Task Should_throw_an_error_when_adding_todo_and_list_already_has_maximum_number()
        {
            // arrange
            var todoListId = await ArrangeTodoListExistWithNoTodos();

            for (var i = 0; i < TodoList.MaxNumberOfTodosNotDoneAllowed; i++)
                await _addTodoUseCase.Invoke(new AddTodoCommand(todoListId,
                    TodosFakeData.CreateTodoDescription()));

            var addTodoCommand = new AddTodoCommand(todoListId, TodosFakeData.CreateTodoDescription());
            // act / assert
            var exception = await Assert.ThrowsAsync<DomainException>(() =>
                _addTodoUseCase.Invoke(addTodoCommand));
            var exceptionData = exception.Data;
            Assert.Equal(TodoList.MaxNumberOfTodosNotDoneAllowed.ToString(),
                exceptionData["CurrentNumberOfTodos"]);
        }


        private async Task<TodoListId> ArrangeTodoListExistWithNoTodos()
        {
            var ownerId = await CreateUser();
            var createTodoListRequest = FakeCommandGenerator.FakeCreateTodoListCommand(ownerId);
            await _todoListRepository.RemoveByName(ownerId, createTodoListRequest.TodoListName);
            return await _createTodoListUseCase.Invoke(createTodoListRequest);
        }
        

        private async Task<TodoListId> ArrangeTodoListDoesNotExist()
        {
            var id = new TodoListId();
            await _todoListRepository.RemoveById(id);
            return id;
        }

        private Task<UserId> CreateUser()
        {
            return _createUserUseCase.Invoke(FakeCommandGenerator.FakeCreateUserCommand());
        }
    }
}