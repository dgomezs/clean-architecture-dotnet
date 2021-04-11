﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Services.Tests.TestDoubles;
using Autofac.Extras.Moq;
using Domain.Entities;
using Domain.ValueObjects;
using Xunit;

namespace Application.Services.Tests.MarkTodoAsDone
{
    public class MarkTodoAsDoneTest
    {
        private AutoMock _mock;
        private readonly InMemoryTodoListRepository _todoListRepository;
        private readonly InMemoryEventPublisher _eventPublisher;
        private readonly IMarkTodoDoneUseCase _markTodoDoneUseCase;

        public MarkTodoAsDoneTest()
        {
            _mock = DiConfig.GetMock();
            _todoListRepository = _mock.Create<InMemoryTodoListRepository>();
            _eventPublisher = _mock.Create<InMemoryEventPublisher>();
            _markTodoDoneUseCase = _mock.Create<IMarkTodoDoneUseCase>();
        }

        [Fact]
        public async Task Should_mark_todo_done_when_list_and_todo_exists_and_todo_not_done_yet()
        {
            // arrange
            var numberOfTodosNotDone = 1;
            var existingTodoList = await ArrangeTodoListExistsWithTodosNotDone(numberOfTodosNotDone);
            var todo = existingTodoList.Todos.First();
            // act
            await _markTodoDoneUseCase.Invoke(new MarkTodoDoneCommand(todo.Id));
            // assert
            var todoListAfterUpdate = await _todoListRepository.GetByTodoId(todo.Id) ?? throw new Exception();
            var todoAfterUpdate = todoListAfterUpdate.Todos.First(t => todo.Id.Equals(t.Id));
            Assert.True(todoAfterUpdate.IsDone());
        }

        private async Task<TodoList> ArrangeTodoListExistsWithTodosNotDone(int numberOfTodosNotDone)
        {
            var todoList = MockDataGenerator.CreateTodoListWithNumberTodosNotDone(numberOfTodosNotDone);
            await _todoListRepository.Save(todoList);
            return todoList;
        }
    }

    public record MarkTodoDoneCommand(TodoId TodoId)
    {
    }

    internal interface IMarkTodoDoneUseCase
    {
        Task Invoke(MarkTodoDoneCommand markTodoDoneCommand);
    }
}