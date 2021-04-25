using System.Net.Http;
using System.Threading.Tasks;
using Application.Services.Users.UseCases.CreateUser;
using CleanArchitecture.TodoList.WebApi.Tests.Config;
using Domain.Users.ValueObjects;
using FakeTestData;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WebApi.Controllers.Users;
using Xunit;

namespace CleanArchitecture.TodoList.WebApi.Tests.CreateUser
{
    public class CreateUserTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly Mock<ICreateUserUseCase> _createUserUseCase;
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public CreateUserTest(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _createUserUseCase = new Mock<ICreateUserUseCase>();
        }

        [Fact]
        public async Task Should_return_id_of_a_user()
        {
            // Arrange
            var expectedId = new UserId();
            var email = UserFakeData.CreateEmail().Value;
            var personName = UserFakeData.CreatePersonName();
            RestCreateUserRequest createUserRequest = new(personName.FirstName, personName.LastName, email);

            MockControllerResponse(expectedId, createUserRequest);
            // act
            var response = await SendCreateUserCommand(createUserRequest);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var body = await response.Content.ReadAsStringAsync();
            body.Should().Be(expectedId.Value.ToString());
        }


        private async Task<HttpResponseMessage> SendCreateUserCommand(RestCreateUserRequest createUserRequest)
        {
            var client = ConfigureClient();
            // Act
            var stringContent = ContentHelper.GetStringContent(createUserRequest);
            var response = await client.PostAsync("/users", stringContent);
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

        private void MockControllerResponse(UserId expectedId, RestCreateUserRequest restCreateUserRequest)
        {
            var (firstName, lastName, email) = restCreateUserRequest;

            _createUserUseCase.Setup(m =>
                    m.Invoke(It.Is<CreateUserCommand>(c =>
                        email.Equals(c.Email.Value)
                        && firstName.Equals(c.Name.FirstName)
                        && lastName.Equals(c.Name.LastName)
                    )))
                .ReturnsAsync(expectedId);
        }
    }
}