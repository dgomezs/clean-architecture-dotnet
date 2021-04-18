using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Services.Todos.Repositories;
using Autofac;
using FakeTestData;
using Persistence.Tests.Fixtures;
using Xunit;

namespace Persistence.Tests.Todos.PersistTodoList
{
    [Collection("DB")]
    public class PersistTodoListTest
    {
        private readonly ITodoListRepository _todoListRepository;

        public PersistTodoListTest(DbFixture dbFixture) =>
            _todoListRepository = dbFixture.Container.Resolve<ITodoListRepository>();

        [Fact]
        public async Task Should_persist_new_created_todo_list()
        {
            // arrange
            var todoList = TodoListFakeData.CreateTodoList();
            var todoListId = todoList.Id;
            // act
            await _todoListRepository.Save(todoList);
            var todoListPersisted = await _todoListRepository.GetById(todoListId) ?? throw new Exception();

            // assert
            Assert.Equal(todoList.Name, todoListPersisted.Name);
        }

        [Fact]
        public async Task Should_persist_new_created_todo_list_with_some_todos()
        {
            // arrange
            const int totalNumberOfTodos = 3;
            const int numberOfTodosDone = 2;
            var todoList = TodoListFakeData.CreateTodoList(totalNumberOfTodos, numberOfTodosDone);
            // act
            await _todoListRepository.Save(todoList);
            var todoListPersisted = await _todoListRepository.GetById(todoList.Id) ?? throw new Exception();
            var todosDonePersisted = todoListPersisted.Todos.Where(t => t.Done);
            // assert
            Assert.Equal(todoList.Name, todoListPersisted.Name);
            Assert.Equal(totalNumberOfTodos, todoListPersisted.Todos.Count());
            Assert.Equal(numberOfTodosDone, todosDonePersisted.Count());
        }
    }
}