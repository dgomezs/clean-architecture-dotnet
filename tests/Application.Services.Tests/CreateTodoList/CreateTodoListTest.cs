using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Services.Errors;
using Application.Services.Tests.TestDoubles;
using Application.Services.UseCases.CreateTodoList;
using Autofac.Extras.Moq;
using Domain.Events;
using Domain.ValueObjects;
using Xunit;

namespace Application.Services.Tests.CreateTodoList
{
    public class CreateTodoListTest
    {
        private readonly ICreateTodoListUseCase _createTodoListUseCase;
        private readonly InMemoryTodoListRepository _todoListRepository;
        private readonly AutoMock _mock;
        private readonly InMemoryEventPublisher _eventPublisher;

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
            var createTodoListRequest = MockDataGenerator.CreateTodoList();
            var todoListName = createTodoListRequest.TodoListName;
            await ArrangeTodoListDoesNotExist(todoListName);
            // act
            var id = await _createTodoListUseCase.Invoke(createTodoListRequest);
            // assert
            var todoList = await _todoListRepository.GetById(id) ?? throw new Exception();
            Assert.Equal(todoListName, todoList.Name);
            Assert.Equal(id, todoList.Id);
            IEnumerable<DomainEvent> domainEvents = _eventPublisher.Events;
            Assert.Single(domainEvents);
            Assert.Collection(domainEvents, d => Assert.True(d is TodoListCreatedEvent));
        }


        [Fact]
        public async Task Should_not_create_new_todolist_when_one_by_same_name_exists()
        {
            // arrange
            var createTodoListRequest = MockDataGenerator.CreateTodoList();
            await ArrangeTodoListDoesNotExist(createTodoListRequest.TodoListName);
            await _createTodoListUseCase.Invoke(createTodoListRequest);

            // act
            // create another list by the same
            await Assert.ThrowsAsync<TodoListAlreadyExistsException>(() =>
                _createTodoListUseCase.Invoke(createTodoListRequest));
        }


        private async Task ArrangeTodoListDoesNotExist(TodoListName todoListName)
        {
            await _todoListRepository.RemoveByName(todoListName);
        }
    }
}