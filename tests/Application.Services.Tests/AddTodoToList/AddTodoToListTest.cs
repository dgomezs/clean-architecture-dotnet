using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services.Tests.TestDoubles;
using Application.Services.UseCases.AddTodo;
using Application.Services.UseCases.CreateTodoList;
using Autofac.Extras.Moq;
using Domain.Entities;
using Domain.Errors;
using Domain.Events;
using Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Application.Services.Tests.AddTodoToList
{
    public class AddTodoToListTest
    {
        private AutoMock _mock;
        private readonly InMemoryTodoListRepository _todoListRepository;
        private readonly ICreateTodoListUseCase _createTodoListUseCase;
        private readonly IAddTodoUseCase _addTodoUseCase;
        private readonly InMemoryEventPublisher _eventPublisher;

        public AddTodoToListTest()
        {
            _mock = DiConfig.GetMock();
            _todoListRepository = _mock.Create<InMemoryTodoListRepository>();
            _eventPublisher = _mock.Create<InMemoryEventPublisher>();
            _createTodoListUseCase = _mock.Create<ICreateTodoListUseCase>();
            _addTodoUseCase = _mock.Create<IAddTodoUseCase>();
        }

        [Fact]
        public async Task Should_add_a_todo_to_a_list_when_list_exists_and_max_todos_not_reached()
        {
            // arrange
            var createTodoListRequest = MockDataGenerator.CreateTodoList();
            var todoListId = await ArrangeTodoListExistWithNoTodos(createTodoListRequest);
            var todoDescription = MockDataGenerator.CreateTodoDescription();
            var addTodoCommand = new AddTodoCommand(todoListId, todoDescription);
            // act
            var todoId = await _addTodoUseCase.AddTodo(addTodoCommand);
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
            var createTodoListRequest = MockDataGenerator.CreateTodoList();
            var todoListId = await ArrangeTodoListExistWithNoTodos(createTodoListRequest);
            var todoDescription = MockDataGenerator.CreateTodoDescription();
            var addTodoCommand = new AddTodoCommand(todoListId, todoDescription);
            _eventPublisher.ClearEvents();
            // act
            var todoId = await _addTodoUseCase.AddTodo(addTodoCommand);
            // assert
            IEnumerable<DomainEvent> domainEvents = _eventPublisher.Events;
            Assert.Single(domainEvents);
            var eDomainEvent = domainEvents.Single();
            Assert.True(eDomainEvent is TodoAddedToListEvent);
            var todoAddedToListEvent = (TodoAddedToListEvent) eDomainEvent;
            Assert.Equal(todoDescription, todoAddedToListEvent.Todo.Description);
            Assert.Equal(todoListId, todoAddedToListEvent.TodoListId);
        }


        [Fact]
        public async Task Should_throw_an_error_when_todo_list_does_not_exist()
        {
            // arrange
            var createTodoListRequest = MockDataGenerator.CreateTodoList();
            var todoListId = await ArrangeTodoListDoesNotExist(createTodoListRequest);
            var addTodoCommand = new AddTodoCommand(todoListId, MockDataGenerator.CreateTodoDescription());
            // act / assert
            var exception = await Assert.ThrowsAsync<DomainException>(() =>
                _addTodoUseCase.AddTodo(addTodoCommand));
            var exceptionData = exception.Data;
            Assert.Equal(todoListId.Value.ToString(), exceptionData["TodoListId"]);
        }

        [Fact]
        public async Task Should_throw_an_error_when_adding_todo_and_list_already_has_maximum_number()
        {
            // arrange
            var createTodoListRequest = MockDataGenerator.CreateTodoList();
            var todoListId = await ArrangeTodoListExistWithNoTodos(createTodoListRequest);

            for (var i = 0; i < TodoList.MaxNumberOfTodosNotDoneAllowed; i++)
            {
                await _addTodoUseCase.AddTodo(new AddTodoCommand(todoListId,
                    MockDataGenerator.CreateTodoDescription()));
            }

            var addTodoCommand = new AddTodoCommand(todoListId, MockDataGenerator.CreateTodoDescription());
            // act / assert
            var exception = await Assert.ThrowsAsync<DomainException>(() =>
                _addTodoUseCase.AddTodo(addTodoCommand));
            var exceptionData = exception.Data;
            Assert.Equal(TodoList.MaxNumberOfTodosNotDoneAllowed.ToString(), exceptionData["CurrentNumberOfTodos"]);
        }


        private async Task<TodoListId> ArrangeTodoListExistWithNoTodos(
            CreateTodoListCommand todoListName)
        {
            await _todoListRepository.RemoveByName(todoListName.TodoListName);
            return await _createTodoListUseCase.Invoke(todoListName);
        }

        private async Task<TodoListId> ArrangeTodoListDoesNotExist(CreateTodoListCommand todoListName)
        {
            var id = await _createTodoListUseCase.Invoke(todoListName);
            await _todoListRepository.RemoveByName(todoListName.TodoListName);
            return id;
        }
    }
}