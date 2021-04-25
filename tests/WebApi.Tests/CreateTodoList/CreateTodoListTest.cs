using System;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Services.Todos.UseCases.CreateTodoList;
using CleanArchitecture.TodoList.WebApi.Tests.Config;
using Domain.Shared.Errors;
using Domain.Todos.ValueObjects;
using Domain.Users.ValueObjects;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WebApi;
using WebApi.Controllers.CreateTodoList;
using Xunit;
using static LanguageExt.Prelude;

namespace CleanArchitecture.TodoList.WebApi.Tests.CreateTodoList
{
    public class CreateTodoListTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly Mock<ICreateTodoListUseCase> _createTodoListUseCaseMock;
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public CreateTodoListTest(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _createTodoListUseCaseMock = new Mock<ICreateTodoListUseCase>();
        }

        [Fact]
        public async Task Should_return_id_of_a_new_list()
        {
            // Arrange
            var expectedId = new TodoListId();
            const string listName = "todoList";
            var ownerId = new UserId();
            MockControllerResponse(expectedId, ownerId, listName);
            // act
            var response = await SendCreateTodoListCommand(ownerId, listName);

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
            const string listName = "todoList";
            var ownerId = new UserId();
            var expectedErrorResponse = MockControllerErrorResponse(error, ownerId, listName);

            // act
            var response = await SendCreateTodoListCommand(ownerId, listName);
            // assert
            await ErrorAssertionUtils.AssertError(response, expectedErrorResponse);
        }

        private RestErrorResponse MockControllerErrorResponse(Error error, UserId ownerId, string listName)
        {
            var expectedErrorResponse = new RestErrorResponse((int) HttpStatusCode.InternalServerError,
                error.ErrorKey, error.Message);

            _createTodoListUseCaseMock.Setup(m =>
                    m.Invoke(It.Is(Match(ownerId, listName))))
                .ThrowsAsync(new DomainException(error));

            _createTodoListUseCaseMock.Setup(m =>
                m.InvokeWithErrors(It.Is(Match(ownerId, listName)))).ReturnsAsync(Left(error));
            return expectedErrorResponse;
        }

        private async Task<HttpResponseMessage> SendCreateTodoListCommand(UserId ownerId, string todoListName)
        {
            var client = ConfigureClient();
            RestCreateTodoListRequest createTodoListRequest = new(todoListName);

            // Act
            var stringContent = ContentHelper.GetStringContent(createTodoListRequest);
            stringContent.Headers.Add("OwnerId", ownerId.Value.ToString());
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

        private void MockControllerResponse(TodoListId expectedId, UserId ownerId, string listName)
        {
            _createTodoListUseCaseMock.Setup(m =>
                    m.Invoke(It.Is(Match(ownerId, listName))))
                .ReturnsAsync(expectedId);

            _createTodoListUseCaseMock.Setup(m =>
                    m.InvokeWithErrors(It.Is(Match(ownerId, listName))))
                .ReturnsAsync(Right(expectedId));
        }


        private static Expression<Func<CreateTodoListCommand, bool>> Match(UserId ownerId, string listName)
        {
            return c => listName.Equals(c.TodoListName.Name) && ownerId.Equals(c.OwnerId);
        }
    }
}