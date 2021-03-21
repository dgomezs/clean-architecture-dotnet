using System.Net.Http;
using System.Threading.Tasks;
using Application.Services.UseCases.CreateTodoList;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
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
                (Mock<ICreateTodoListUseCase>) _factory.Services.GetRequiredService(
                    typeof(Mock<ICreateTodoListUseCase>));
            createTodoListUseCaseMock.Setup(m =>
                    m.Invoke(It.IsAny<CreateTodoListCommand>()))
                .ReturnsAsync(expectedId);

            RestCreateTodoListRequest createTodoListRequest = new("todolist");

            // Act
            var stringContent = ContentHelper.GetStringContent(createTodoListRequest);
            var response = await _client.PostAsync("/todo-lists", stringContent);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var body = await response.Content.ReadAsStringAsync();
            body.Should().Be(expectedId.ToString());
        }
    }
}