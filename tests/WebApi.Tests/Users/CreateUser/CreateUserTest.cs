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
using WebApi.Auth;
using WebApi.Controllers.Users;
using WebApi.Errors;
using Xunit;

namespace CleanArchitecture.TodoList.WebApi.Tests.Users.CreateUser
{
    public class CreateUserTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly Mock<ICreateUserUseCase> _createUserUseCaseMock;
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly Mock<IAuthService> _authServices;

        public CreateUserTest(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _createUserUseCaseMock = new Mock<ICreateUserUseCase>();
            _authServices = new Mock<IAuthService>();
        }

        [Fact]
        public async Task Should_create_user_when_valid_data_and_has_signed_up_in_auth_system()
        {
            // Arrange
            var createUserRequest = CreateUserRequest();
            var emailAddress = EmailAddress.Create(createUserRequest.Email);
            var expectedId = MockSuccessfulCreateUserResponse(createUserRequest);
            MockUserHasSignedUp(createUserRequest);
            // act
            var response = await SendCreateUserCommand(createUserRequest);
            // Assert
            _authServices.Verify(
                m => m.HasUserSignedUpInAuthSystem(emailAddress)
                , Times.Once);
            _authServices.Verify(m => m.AssignUserId(emailAddress, expectedId), Times.Once);
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var body = await response.Content.ReadAsStringAsync();
            body.Should().Be(expectedId.Value.ToString());
        }

        [Fact]
        public async Task Should_return_validation_error_when_user_is_not_in_auth_system()
        {
            // arrange
            var createUserRequest = CreateUserRequest();
            MockUserHasNotSignedUpYet(createUserRequest);

            // act
            var response = await SendCreateUserCommand(createUserRequest);

            // assert
            var userHasNotSignedUpError =
                new UserHasNotSignedUpError(EmailAddress.Create(createUserRequest.Email));
            var expectedErrorResponse = new RestErrorResponse((int) HttpStatusCode.BadRequest,
                userHasNotSignedUpError.Code, new List<Error>(), userHasNotSignedUpError.Message);
            await ErrorAssertionUtils.AssertError(response, expectedErrorResponse);
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
            RestCreateUserRequest createUserRequest = new(firstName, lastName, email);
            MockSuccessfulCreateUserResponse(createUserRequest);
            // act
            var response = await SendCreateUserCommand(createUserRequest);
            // assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_return_appropriate_error_code_when_creating_user_fails()
        {
            var createUserRequest = CreateUserRequest();
            var userAlreadyExistsError =
                new UserAlreadyExistsError(EmailAddress.Create(createUserRequest.Email));
            var expectedException = new DomainException(userAlreadyExistsError);

            MockFailUseCaseResponse(createUserRequest, expectedException);
            MockUserHasSignedUp(createUserRequest);
            // act
            var response = await SendCreateUserCommand(createUserRequest);
            // assert
            var expectedErrorResponse = new RestErrorResponse((int) HttpStatusCode.Conflict,
                userAlreadyExistsError.Code, new List<Error>(), userAlreadyExistsError.Message);
            await ErrorAssertionUtils.AssertError(response, expectedErrorResponse);
        }


        private async Task<HttpResponseMessage> SendCreateUserCommand(RestCreateUserRequest createUserRequest)
        {
            var client = ConfigureClient();
            // Act
            var stringContent = HttpRequestHelper.GetStringContent(createUserRequest);
            var response = await client.PostAsync(ControllerTestingConstants.UsersPath, stringContent);
            return response;
        }

        private HttpClient ConfigureClient()
        {
            return _factory.WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddScoped(_ => _createUserUseCaseMock.Object);
                        services.AddScoped(_ => _authServices.Object);
                    });
                })
                .CreateClient();
        }

        private UserId MockSuccessfulCreateUserResponse(RestCreateUserRequest restCreateUserRequest)
        {
            var (firstName, lastName, email) = restCreateUserRequest;

            var expectedId = new UserId();
            _createUserUseCaseMock.Setup(m =>
                    m.Invoke(It.Is(MatchUser(email, firstName, lastName)
                    )))
                .ReturnsAsync(expectedId);
            return expectedId;
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

            _createUserUseCaseMock.Setup(m =>
                    m.Invoke(It.Is(MatchUser(email, firstName, lastName)
                    )))
                .ThrowsAsync(exception);
        }

        private static RestCreateUserRequest CreateUserRequest()
        {
            var email = UserFakeData.CreateEmail().Value;
            var personName = UserFakeData.CreatePersonName();
            return new RestCreateUserRequest(personName.FirstName, personName.LastName, email);
        }

        private void MockUserHasNotSignedUpYet(RestCreateUserRequest createUserRequest)
        {
            _authServices.Setup(m =>
                    m.HasUserSignedUpInAuthSystem(EmailAddress.Create(createUserRequest.Email)))
                .ReturnsAsync(false);
        }

        private void MockUserHasSignedUp(RestCreateUserRequest createUserRequest)
        {
            _authServices.Setup(m => m.HasUserSignedUpInAuthSystem(EmailAddress.Create(createUserRequest.Email)))
                .ReturnsAsync(true);
        }
    }
}