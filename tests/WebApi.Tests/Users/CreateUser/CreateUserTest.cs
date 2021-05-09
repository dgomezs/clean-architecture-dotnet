﻿using System;
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
using WebApi.Auth.UserManagement;
using WebApi.Controllers.Users;
using Xunit;

namespace CleanArchitecture.TodoList.WebApi.Tests.Users.CreateUser
{
    public class CreateUserTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly Mock<ICreateUserUseCase> _createUserUseCaseMock;
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly Mock<IUserManager> _userManagerMock;

        public CreateUserTest(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _createUserUseCaseMock = new Mock<ICreateUserUseCase>();
            _userManagerMock = new Mock<IUserManager>();
        }

        [Fact]
        public async Task Should_create_user_when_valid_data_and_exists_in_auth_system()
        {
            // Arrange
            var expectedId = new UserId();
            var email = UserFakeData.CreateEmail().Value;
            var personName = UserFakeData.CreatePersonName();
            RestCreateUserRequest createUserRequest = new(personName.FirstName, personName.LastName, email);

            MockSuccessfulCreateUserResponse(expectedId, createUserRequest);
            // act
            var response = await SendCreateUserCommand(createUserRequest);

            // Assert
            _userManagerMock.Verify(m => m.HasUserSignedUpInAuthSystem(EmailAddress.Create(email)), Times.Once);
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
            MockSuccessfulCreateUserResponse(expectedId, createUserRequest);
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
                        services.AddScoped(_ => _userManagerMock.Object);
                    });
                })
                .CreateClient();
        }

        private void MockSuccessfulCreateUserResponse(UserId expectedId,
            RestCreateUserRequest restCreateUserRequest)
        {
            var (firstName, lastName, email) = restCreateUserRequest;

            _userManagerMock.Setup(m => m.HasUserSignedUpInAuthSystem(EmailAddress.Create(email)))
                .ReturnsAsync(true);

            _createUserUseCaseMock.Setup(m =>
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

            _createUserUseCaseMock.Setup(m =>
                    m.Invoke(It.Is(MatchUser(email, firstName, lastName)
                    )))
                .ThrowsAsync(exception);
        }
    }
}