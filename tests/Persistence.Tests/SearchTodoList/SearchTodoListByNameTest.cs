﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Services.Repositories;
using Application.Services.UseCases.SearchTodoListByName;
using Autofac;
using Domain.Todos.Entities;
using Persistence.Tests.FakeData;
using Persistence.Tests.Fixtures;
using Xunit;

namespace Persistence.Tests.SearchTodoList
{
    [Collection("DB")]
    public class SearchTodoListByNameTest
    {
        private readonly ITodoListRepository _todoListRepository;
        private readonly ISearchByNameTodoListUseCase _searchByNameTodoListUseCase;

        public SearchTodoListByNameTest(DbFixture dbFixture)
        {
            _todoListRepository = dbFixture.Container.Resolve<ITodoListRepository>();
            _searchByNameTodoListUseCase = dbFixture.Container.Resolve<ISearchByNameTodoListUseCase>();
        }

        [Fact]
        public async Task Should_find_all_todo_list_names_starting_by()
        {
            // arrange
            const string startingBy = "sh";
            const string nonStartingBy = "pr";
            const int size = 5;
            var todoListsToBeFound = TodoListMockData.CreateTodoList(startingBy, size);
            var todoListNotToBeFound = TodoListMockData.CreateTodoList(nonStartingBy, size);
            await PersistTodoList(todoListsToBeFound);
            await PersistTodoList(todoListNotToBeFound);
            // act
            var foundTodoLists = await _searchByNameTodoListUseCase.SearchByName(startingBy);
            // assert
            Assert.Equal(todoListsToBeFound.Count, foundTodoLists.Count);
        }

        private async Task PersistTodoList(List<TodoList> todoListsToBeFound)
        {
            foreach (var todoList in todoListsToBeFound)
            {
                await _todoListRepository.Save(todoList);
            }
        }
    }
}