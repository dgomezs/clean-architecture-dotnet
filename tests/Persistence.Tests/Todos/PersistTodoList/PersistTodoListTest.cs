using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Services.Todos.Repositories;
using Autofac;
using Domain.Todos.Entities;
using Domain.Todos.ValueObjects;
using FakeTestData;
using Persistence.Tests.Fixtures;
using Persistence.Tests.Helpers;
using Xunit;

namespace Persistence.Tests.Todos.PersistTodoList
{
    [Collection("DB")]
    public class PersistTodoListTest
    {
        private readonly ITodoListRepository _todoListRepository;
        private readonly UserArrangeHelper _userArrangeHelper;

        public PersistTodoListTest(DbFixture dbFixture)
        {
            _todoListRepository = dbFixture.Container.Resolve<ITodoListRepository>();
            _userArrangeHelper = dbFixture.Container.Resolve<UserArrangeHelper>();
        }

        [Fact]
        public async Task Should_persist_new_created_todo_list()
        {
            // arrange
            var owner = await _userArrangeHelper.CreateUser();
            var todoList = TodoListFakeData.CreateTodoList(owner.Id);
            var todoListId = todoList.Id;
            // act
            await _todoListRepository.Save(todoList);
            var todoListPersisted = await GetById(todoListId);
            // assert
            Assert.Equal(todoList.Name, todoListPersisted.Name);
            Assert.Equal(owner.Id, todoListPersisted.OwnerId);
        }

        [Fact]
        public async Task Should_persist_new_created_todo_list_with_some_todos()
        {
            // arrange
            var owner = await _userArrangeHelper.CreateUser();
            const int totalNumberOfTodos = 3;
            const int numberOfTodosDone = 2;
            var todoList = TodoListFakeData.CreateTodoList(owner.Id, totalNumberOfTodos, numberOfTodosDone);
            // act
            await _todoListRepository.Save(todoList);
            var todoListPersisted = await GetById(todoList.Id);
            var todosDonePersisted = todoListPersisted.Todos.Where(t => t.Done);
            // assert
            Assert.Equal(todoList.Name, todoListPersisted.Name);
            Assert.Equal(totalNumberOfTodos, todoListPersisted.Todos.Count());
            Assert.Equal(numberOfTodosDone, todosDonePersisted.Count());
        }


        private async Task<TodoList> GetById(TodoListId todoListId)
        {
            return await _todoListRepository.GetById(todoListId) ?? throw new Exception();
        }
    }
}