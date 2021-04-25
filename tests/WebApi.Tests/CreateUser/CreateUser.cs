using System.Net.Http;
using System.Threading.Tasks;
using Application.Services.Users.UseCases.CreateUser;
using CleanArchitecture.TodoList.WebApi.Tests.Config;
using Domain.Users.ValueObjects;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WebApi.Controllers.CreateTodoList;
using Xunit;

namespace CleanArchitecture.TodoList.WebApi.Tests.CreateUser
{
    public class CreateUser : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly Mock<ICreateUserUseCase> _createUserUseCase;
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public CreateUser(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _createUserUseCase = new Mock<ICreateUserUseCase>();
        }

        [Fact]
        public async Task Should_return_id_of_a_user()
        {
            // Arrange
            var expectedId = new UserId();
            MockControllerResponse(expectedId);
            // act
            var response = await SendCreateUserCommand("todoList");

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var body = await response.Content.ReadAsStringAsync();
            body.Should().Be(expectedId.Value.ToString());
        }


        private async Task<HttpResponseMessage> SendCreateUserCommand(string todoListName)
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
                        services.AddScoped(x => _createUserUseCase.Object);
                    });
                })
                .CreateClient();
        }

        private void MockControllerResponse(UserId expectedId)
        {
            _createUserUseCase.Setup(m =>
                    m.Invoke(It.IsAny<CreateUserCommand>()))
                .ReturnsAsync(expectedId);
        }
    }
}