using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Services.Tests.TestDoubles;
using Application.Services.Todos.UseCases.MarkTodoAsDone;
using Autofac.Extras.Moq;
using Domain.Todos.Entities;
using Domain.Users.ValueObjects;
using FakeTestData;
using Xunit;

namespace Application.Services.Tests.Todos.MarkTodoAsDone
{
    public class MarkTodoAsDoneTest
    {
        private readonly IMarkTodoDoneUseCase _markTodoDoneUseCase;
        private readonly AutoMock _mock;
        private readonly InMemoryTodoListRepository _todoListRepository;

        public MarkTodoAsDoneTest()
        {
            _mock = DiConfig.GetMock();
            _todoListRepository = _mock.Create<InMemoryTodoListRepository>();
            _markTodoDoneUseCase = _mock.Create<IMarkTodoDoneUseCase>();
        }

        [Fact]
        public async Task Should_mark_todo_done_when_list_and_todo_exists_and_todo_not_done_yet()
        {
            // arrange
            const int numberOfTodosNotDone = 1;
            var existingTodoList = await ArrangeTodoListExistsWithTodosNotDone(numberOfTodosNotDone);
            var todo = existingTodoList.Todos.First();
            // act
            await _markTodoDoneUseCase.Invoke(new MarkTodoDoneCommand(todo.Id));
            // assert
            var todoListAfterUpdate = await _todoListRepository.GetByTodoId(todo.Id) ?? throw new Exception();
            var todoAfterUpdate = todoListAfterUpdate.Todos.First(t => todo.Id.Equals(t.Id));
            Assert.True(todoAfterUpdate.Done);
        }

        private async Task<TodoList> ArrangeTodoListExistsWithTodosNotDone(
            int numberOfTodosNotDone)
        {
            var todoList =
                TodoListFakeData.CreateTodoListWithNumberTodosNotDone(new UserId(), numberOfTodosNotDone);
            await _todoListRepository.Save(todoList);
            return todoList;
        }
    }
}