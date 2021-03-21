using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Services.UseCases.CreateTodoList;
using Domain.Errors;
using FluentAssertions;
using FluentAssertions.Json;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json.Linq;
using WebApi.Controllers;
using Xunit;

namespace CleanArchitecture.TodoList.WebApi.Tests.CreateTodoList
{
    public class CreateTodoListTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private HttpClient _client;

        public CreateTodoListTest(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task Should_return_id_of_a_new_list()
        {
            // Arrange
            const int expectedId = 3;
            var createTodoListUseCaseMock =
                GetCreateTodoListUseCase();
            createTodoListUseCaseMock.Setup(m =>
                    m.Invoke(It.IsAny<CreateTodoListCommand>()))
                .ReturnsAsync(expectedId);

            // act
            var response = await SendCreateTodoListCommand("todoList");

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var body = await response.Content.ReadAsStringAsync();
            body.Should().Be(expectedId.ToString());
        }

        [Fact]
        public async Task Should_return_error_if_todo_list_already_exists()
        {
            // arrange
            var createTodoListUseCaseMock =
                GetCreateTodoListUseCase();
            const string errorKey = "TestingErrorKey";
            const string errorMessage = "ErrorMessage";

            createTodoListUseCaseMock.Setup(m =>
                    m.Invoke(It.IsAny<CreateTodoListCommand>()))
                .ThrowsAsync(new DomainException(errorKey, errorMessage));

            // act
            var response = await SendCreateTodoListCommand("todoList");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            var body = JToken.Parse(await response.Content.ReadAsStringAsync());
            var expectedResult =
                JToken.Parse(
                    $"{{\"message\":\"{errorMessage}\",\"status\":500,\"errors\":[],\"errorKey\":\"{errorKey}\"}}");


            body.Should().BeEquivalentTo(expectedResult);
        }

        private async Task<HttpResponseMessage> SendCreateTodoListCommand(string todoListName)
        {
            RestCreateTodoListRequest createTodoListRequest = new(todoListName);

            // Act
            var stringContent = ContentHelper.GetStringContent(createTodoListRequest);
            var response = await _client.PostAsync("/todo-lists", stringContent);
            return response;
        }

        private Mock<ICreateTodoListUseCase> GetCreateTodoListUseCase()
        {
            return (Mock<ICreateTodoListUseCase>) _factory.Services.GetRequiredService(
                typeof(Mock<ICreateTodoListUseCase>));
        }
    }
}