using System;
using System.Threading.Tasks;
using Application.Services.Repositories;
using Autofac;
using Persistence.Tests.FakeData;
using Persistence.Tests.Fixtures;
using Xunit;

namespace Persistence.Tests.PersistTodoList
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
            var todoList = TodoListMockData.CreateTodoList();
            var todoListId = todoList.Id;
            // act
            await _todoListRepository.Save(todoList);
            var todoListPersisted = await _todoListRepository.GetById(todoListId) ?? throw new Exception();

            // assert
            Assert.Equal(todoList.Name, todoListPersisted.Name);
        }
    }
}