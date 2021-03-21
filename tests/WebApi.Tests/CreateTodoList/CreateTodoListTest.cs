using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Services.UseCases.CreateTodoList;
using CleanArchitecture.TodoList.WebApi.Tests.Config;
using Domain.Errors;
using FluentAssertions;
using FluentAssertions.Json;
using Microsoft.AspNetCore.TestHost;
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

        public CreateTodoListTest(CustomWebApplicationFactory<Startup> factory) =>
            _factory = factory;

        [Fact]
        public async Task Should_return_id_of_a_new_list()
        {
            // Arrange
            const int expectedId = 3;
            var createTodoListUseCaseMock = new Mock<ICreateTodoListUseCase>();

            var client = ConfigureClient(createTodoListUseCaseMock);

            createTodoListUseCaseMock.Setup(m =>
                    m.Invoke(It.IsAny<CreateTodoListCommand>()))
                .ReturnsAsync(expectedId);

            // act
            var response = await SendCreateTodoListCommand(client, "todoList");

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var body = await response.Content.ReadAsStringAsync();
            body.Should().Be(expectedId.ToString());
        }

        [Fact]
        public async Task Should_return_error_if_todo_list_already_exists()
        {
            // arrange
            var createTodoListUseCaseMock = new Mock<ICreateTodoListUseCase>();
            var client = ConfigureClient(createTodoListUseCaseMock);

            const string errorKey = "TestingErrorKey";
            const string errorMessage = "ErrorMessage";
            var errors = new List<Error>()
            {
                new("ddd")
            };

            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.InternalServerError;

            createTodoListUseCaseMock.Setup(m =>
                    m.Invoke(It.IsAny<CreateTodoListCommand>()))
                .ThrowsAsync(new DomainException(errorKey, errorMessage, errors));

            // act
            var response = await SendCreateTodoListCommand(client, "todoList");

            // assert
            response.StatusCode.Should().Be(expectedHttpStatusCode);
            var body = JToken.Parse(await response.Content.ReadAsStringAsync());
            var expectedResult = ErrorAssertionUtils.ExpectedErrorResult(errorMessage, errorKey,
                expectedHttpStatusCode, errors);


            body.Should().BeEquivalentTo(expectedResult);
        }

        private async Task<HttpResponseMessage> SendCreateTodoListCommand(HttpClient client, string todoListName)
        {
            RestCreateTodoListRequest createTodoListRequest = new(todoListName);

            // Act
            var stringContent = ContentHelper.GetStringContent(createTodoListRequest);
            var response = await client.PostAsync("/todo-lists", stringContent);
            return response;
        }

        private HttpClient ConfigureClient(Mock<ICreateTodoListUseCase> createTodoListUseCaseMock)
        {
            return _factory.WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddScoped(x => createTodoListUseCaseMock.Object);
                    });
                })
                .CreateClient();
        }
    }
}