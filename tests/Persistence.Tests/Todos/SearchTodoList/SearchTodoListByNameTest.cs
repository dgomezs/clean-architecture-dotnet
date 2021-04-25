using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services.Todos.Repositories;
using Application.Services.Todos.UseCases.SearchTodoListByName;
using Autofac;
using Domain.Todos.Entities;
using Domain.Users.ValueObjects;
using FakeTestData;
using Persistence.Tests.Fixtures;
using Persistence.Tests.Helpers;
using Xunit;

namespace Persistence.Tests.Todos.SearchTodoList
{
    [Collection("DB")]
    public class SearchTodoListByNameTest
    {
        private readonly ISearchByNameTodoListUseCase _searchByNameTodoListUseCase;
        private readonly ITodoListRepository _todoListRepository;
        private readonly UserArrangeHelper _userArrangeHelper;

        public SearchTodoListByNameTest(DbFixture dbFixture)
        {
            _todoListRepository = dbFixture.Container.Resolve<ITodoListRepository>();
            _searchByNameTodoListUseCase = dbFixture.Container.Resolve<ISearchByNameTodoListUseCase>();
            _userArrangeHelper = dbFixture.Container.Resolve<UserArrangeHelper>();
        }

        [Fact]
        public async Task Should_find_all_todo_list_names_starting_by()
        {
            // arrange
            var owner = await _userArrangeHelper.CreateUser();
            const string startingBy = "sh";
            const string nonStartingBy = "pr";
            const int size = 3;
            var todoListsToBeFound = await CreateTodoLists(owner.Id, startingBy, size);
            var todoListNotToBeFound = await CreateTodoLists(owner.Id, nonStartingBy, size);
            // act
            var foundTodoLists = await _searchByNameTodoListUseCase.SearchByName(owner.Id, startingBy);
            // assert
            ContainsAllValues(foundTodoLists, todoListsToBeFound);
            DoesNotContainAnyValue(foundTodoLists, todoListNotToBeFound);
        }

        private static void DoesNotContainAnyValue(IEnumerable<TodoListReadModel> foundTodoLists,
            IEnumerable<TodoList> todoListNotToBeFound)
        {
            var foundTodoListsIds = foundTodoLists.Select(_ => _.Id).ToHashSet();
            var todoListsNotToBeFoundIds = todoListNotToBeFound.Select(_ => _.Id.Value).ToHashSet();
            Assert.False(foundTodoListsIds.IsSubsetOf(todoListsNotToBeFoundIds));
        }

        private static void ContainsAllValues(IEnumerable<TodoListReadModel> foundTodoLists,
            IEnumerable<TodoList> todoListsToBeFound)
        {
            var foundTodoListsIds = foundTodoLists.Select(_ => _.Id).ToHashSet();
            var todoListsToBeFoundIds = todoListsToBeFound.Select(_ => _.Id.Value).ToHashSet();
            Assert.True(foundTodoListsIds.IsSubsetOf(todoListsToBeFoundIds));
        }

        private async Task PersistTodoList(IEnumerable<TodoList> todoListsToBeFound)
        {
            foreach (var todoList in todoListsToBeFound)
                await _todoListRepository.Save(todoList);
        }

        private async Task<List<TodoList>> CreateTodoLists(UserId ownerId, string namePrefix,
            int numberOfListToCreate)
        {
            var todoLists = TodoListFakeData.CreateTodoList(ownerId, namePrefix, numberOfListToCreate);
            await PersistTodoList(todoLists);
            return todoLists;
        }
    }
}