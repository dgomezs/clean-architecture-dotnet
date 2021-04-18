using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Application.Services.Todos.UseCases.SearchTodoListByName;
using CleanArchitecture.TodoList.WebApi.Tests.Config;
using Domain.Shared.Errors;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WebApi;
using Xunit;

namespace CleanArchitecture.TodoList.WebApi.Tests.SearchTodoListByName
{
    public class SearchTodoListByNameTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private const string TodoListsSearchByName = "todo-lists/search/by-name";
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly Mock<ISearchByNameTodoListUseCase> _searchByNameTodoListUseCase;

        public SearchTodoListByNameTest(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _searchByNameTodoListUseCase = new Mock<ISearchByNameTodoListUseCase>();
        }


        [Fact]
        public async Task Should_find_todo_lists_by_name()
        {
            // arrange
            const string name = "todo";
            var expectedTodoLists = CreateExpectedTodoLists();
            MockControllerResponse(name, expectedTodoLists);
            // act
            var response = await SearchTodoLists(name);
            var foundTodoLists = await response.Content.ReadFromJsonAsync<List<TodoListReadModel>>() ??
                                 throw new Exception();

            // assert
            expectedTodoLists.SequenceEqual(foundTodoLists).Should().BeTrue();
        }

        [Theory]
        [InlineData(null, "NotNullValidator", "")]
        [InlineData("", "None", "")]
        public async Task Should_return_error_if_name_is_invalid(string invalidName, string errorKey,
            string errorMessage)
        {
            var expectedTodoList = new List<TodoListReadModel>();
            MockControllerResponse(invalidName, expectedTodoList);
            // act
            var response = await SearchTodoLists(invalidName);
            // arrange
            var error = new Error(errorKey, errorMessage);

            var expectedErrorResponse = new RestErrorResponse((int) HttpStatusCode.BadRequest,
                error.ErrorKey, new List<Error>(), error.Message);
            // assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }


        private HttpClient ConfigureClient()
        {
            return _factory.WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddScoped(x => _searchByNameTodoListUseCase.Object);
                    });
                })
                .CreateClient();
        }

        private static List<TodoListReadModel> CreateExpectedTodoLists()
        {
            List<TodoListReadModel> result = new();
            result.Add(new TodoListReadModel(Guid.NewGuid(), "dd"));
            return result;
        }

        private async Task<HttpResponseMessage> SearchTodoLists(string name)
        {
            var client = ConfigureClient();

            return await client.GetAsync($"/{TodoListsSearchByName}?name={name}");
        }


        private void MockControllerResponse(string name, List<TodoListReadModel> result)
        {
            _searchByNameTodoListUseCase.Setup(m => m.SearchByName(name))
                .ReturnsAsync(result);
        }
    }
}