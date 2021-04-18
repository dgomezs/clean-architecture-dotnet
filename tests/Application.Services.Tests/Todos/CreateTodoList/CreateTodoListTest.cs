using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services.Tests.TestDoubles;
using Application.Services.Todos.UseCases.CreateTodoList;
using Autofac.Extras.Moq;
using Domain.Shared.Errors;
using Domain.Shared.Events;
using Domain.Todos.Events;
using Domain.Todos.ValueObjects;
using Xunit;
using static Application.Services.Shared.Extensions.EitherExtensions;

namespace Application.Services.Tests.TodoList.CreateTodoList
{
    public class CreateTodoListTest
    {
        private readonly ICreateTodoListUseCase _createTodoListUseCase;
        private readonly InMemoryEventPublisher _eventPublisher;
        private readonly AutoMock _mock;
        private readonly InMemoryTodoListRepository _todoListRepository;

        public CreateTodoListTest()
        {
            _mock = DiConfig.GetMock();
            _todoListRepository = _mock.Create<InMemoryTodoListRepository>();
            _eventPublisher = _mock.Create<InMemoryEventPublisher>();
            _createTodoListUseCase = _mock.Create<ICreateTodoListUseCase>();
        }

        [Fact]
        public async Task Should_create_new_todolist_when_does_not_exist()
        {
            // arrange
            var createTodoListRequest = FakeCommandGenerator.FakeCreateTodoListCommand();
            var todoListName = createTodoListRequest.TodoListName;
            await ArrangeTodoListDoesNotExist(todoListName);
            // act
            var id = await CreateTodoListWithError(createTodoListRequest);
            // assert
            var todoList = await _todoListRepository.GetById(id) ?? throw new Exception();
            Assert.Equal(todoListName, todoList.Name);
            Assert.Equal(id, todoList.Id);
        }

        [Fact]
        public async Task Should_publish_created_todo_list_event_when_a_new_todo_list_is_created()
        {
            // arrange
            var createTodoListRequest = FakeCommandGenerator.FakeCreateTodoListCommand();
            var todoListName = createTodoListRequest.TodoListName;
            await ArrangeTodoListDoesNotExist(todoListName);
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
        public async Task Should_not_create_new_todolist_when_one_by_same_name_exists()
        {
            // arrange
            var createTodoListRequest = FakeCommandGenerator.FakeCreateTodoListCommand();
            await ArrangeTodoListDoesNotExist(createTodoListRequest.TodoListName);
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

        private async Task ArrangeTodoListDoesNotExist(TodoListName todoListName)
        {
            await _todoListRepository.RemoveByName(todoListName);
        }
    }
}