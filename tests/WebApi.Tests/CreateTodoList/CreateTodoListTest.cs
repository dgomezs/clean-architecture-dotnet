using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Services.UseCases.CreateTodoList;
using CleanArchitecture.TodoList.WebApi.Tests.Config;
using Domain.Entities;
using Domain.Errors;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WebApi;
using WebApi.Controllers.CreateTodoList;
using Xunit;

namespace CleanArchitecture.TodoList.WebApi.Tests.CreateTodoList
{
    public class CreateTodoListTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly Mock<ICreateTodoListUseCase> _createTodoListUseCaseMock;

        public CreateTodoListTest(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _createTodoListUseCaseMock = new Mock<ICreateTodoListUseCase>();
        }

        [Fact]
        public async Task Should_return_id_of_a_new_list()
        {
            // Arrange
            var expectedId = new TodoListId(Guid.NewGuid());
            MockControllerResponse(expectedId);
            // act
            var response = await SendCreateTodoListCommand("todoList");

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var body = await response.Content.ReadAsStringAsync();
            body.Should().Be(expectedId.Value.ToString());
        }

        [Fact]
        public async Task Should_return_error_if_todo_list_already_exists()
        {
            // arrange
            var error = new Error("TestingErrorKey", "ErrorMessage");
            var errors = new List<Error>
            {
                new("ddd")
            };
            var expectedErrorResponse = new RestErrorResponse((int) HttpStatusCode.InternalServerError,
                error.ErrorKey, errors, error.Message);

            _createTodoListUseCaseMock.Setup(m =>
                    m.Invoke(It.IsAny<CreateTodoListCommand>()))
                .ThrowsAsync(new DomainException(error, errors));
            // act
            var response = await SendCreateTodoListCommand("todoList");
            // assert
            await ErrorAssertionUtils.AssertError(response, expectedErrorResponse);
        }

        private async Task<HttpResponseMessage> SendCreateTodoListCommand(string todoListName)
        {
            var client = ConfigureClient();
            RestCreateTodoListRequest createTodoListRequest = new(todoListName);

            // Act
            var stringContent = ContentHelper.GetStringContent(createTodoListRequest);
            var response = await client.PostAsync("/todo-lists", stringContent);
            return response;
        }

        private HttpClient ConfigureClient()
        {
            return _factory.WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddScoped(x => _createTodoListUseCaseMock.Object);
                    });
                })
                .CreateClient();
        }

        private void MockControllerResponse(TodoListId expectedId)
        {
            _createTodoListUseCaseMock.Setup(m =>
                    m.Invoke(It.IsAny<CreateTodoListCommand>()))
                .ReturnsAsync(expectedId);
        }
    }
}