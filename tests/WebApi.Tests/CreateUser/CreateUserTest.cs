using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Services.Users.Errors;
using Application.Services.Users.UseCases.CreateUser;
using CleanArchitecture.TodoList.WebApi.Tests.Config;
using Domain.Shared.Errors;
using Domain.Users.ValueObjects;
using FakeTestData;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WebApi;
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
        public async Task Should_create_user_when_no_errors()
        {
            // Arrange
            var expectedId = new UserId();
            var email = UserFakeData.CreateEmail().Value;
            var personName = UserFakeData.CreatePersonName();
            RestCreateUserRequest createUserRequest = new(personName.FirstName, personName.LastName, email);

            MockSuccessfulUseCaseResponse(expectedId, createUserRequest);
            // act
            var response = await SendCreateUserCommand(createUserRequest);
            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var body = await response.Content.ReadAsStringAsync();
            body.Should().Be(expectedId.Value.ToString());
        }

        [Theory]
        [InlineData("", "", "")]
        [InlineData(null, null, null)]
        [InlineData("John", "", "john.doe@email.com")]
        [InlineData("", "Doe", "")]
        [InlineData("John", "Doe", "")]
        public async Task Should_return_validation_error_when_params_are_not_valid(string firstName,
            string lastName, string email)
        {
            // arrange
            var expectedId = new UserId();
            RestCreateUserRequest createUserRequest = new(firstName, lastName, email);
            MockSuccessfulUseCaseResponse(expectedId, createUserRequest);
            // act
            var response = await SendCreateUserCommand(createUserRequest);
            // assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_return_appropriate_error_code_when_creating_user_fails()
        {
            var email = UserFakeData.CreateEmail().Value;
            var personName = UserFakeData.CreatePersonName();
            RestCreateUserRequest createUserRequest = new(personName.FirstName, personName.LastName, email);
            var userAlreadyExistsError = new UserAlreadyExistsError(EmailAddress.Create(email));
            var expectedException = new DomainException(userAlreadyExistsError);

            MockFailUseCaseResponse(createUserRequest, expectedException);
            // act
            var response = await SendCreateUserCommand(createUserRequest);
            // assert
            var expectedErrorResponse = new RestErrorResponse((int) HttpStatusCode.Conflict,
                userAlreadyExistsError.ErrorKey, new List<Error>(), userAlreadyExistsError.Message);
            await ErrorAssertionUtils.AssertError(response, expectedErrorResponse);
        }


        private async Task<HttpResponseMessage> SendCreateUserCommand(RestCreateUserRequest createUserRequest)
        {
            var client = ConfigureClient();
            // Act
            var stringContent = ContentHelper.GetStringContent(createUserRequest);
            var response = await client.PostAsync(ControllerTestingConstants.UsersPath, stringContent);
            return response;
        }

        private HttpClient ConfigureClient()
        {
            return _factory.WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddScoped(_ => _createUserUseCase.Object);
                    });
                })
                .CreateClient();
        }

        private void MockSuccessfulUseCaseResponse(UserId expectedId,
            RestCreateUserRequest restCreateUserRequest)
        {
            var (firstName, lastName, email) = restCreateUserRequest;

            _createUserUseCase.Setup(m =>
                    m.Invoke(It.Is(MatchUser(email, firstName, lastName)
                    )))
                .ReturnsAsync(expectedId);
        }

        private static Expression<Func<CreateUserCommand, bool>> MatchUser(string email, string firstName,
            string lastName)
        {
            return c =>
                email.Equals(c.Email.Value)
                && firstName.Equals(c.Name.FirstName)
                && lastName.Equals(c.Name.LastName);
        }

        private void MockFailUseCaseResponse(RestCreateUserRequest restCreateUserRequest,
            Exception exception)
        {
            var (firstName, lastName, email) = restCreateUserRequest;

            _createUserUseCase.Setup(m =>
                    m.Invoke(It.Is(MatchUser(email, firstName, lastName)
                    )))
                .ThrowsAsync(exception);
        }
    }
}