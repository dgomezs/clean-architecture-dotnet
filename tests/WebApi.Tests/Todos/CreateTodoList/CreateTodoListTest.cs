using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Services.Todos.UseCases.CreateTodoList;
using Application.Services.Users.Repositories;
using CleanArchitecture.TodoList.WebApi.Tests.Config;
using Domain.Shared.Errors;
using Domain.Todos.ValueObjects;
using Domain.Users.Entities;
using Domain.Users.ValueObjects;
using FakeTestData;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WebApi;
using WebApi.Authorization;
using WebApi.Controllers.Todos.TodoList;
using Xunit;
using static LanguageExt.Prelude;

namespace CleanArchitecture.TodoList.WebApi.Tests.Todos.CreateTodoList
{
    public class CreateTodoListTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly Mock<ICreateTodoListUseCase> _createTodoListUseCaseMock;
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly Mock<IUserRepository> _userRepository;

        public CreateTodoListTest(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _userRepository = new Mock<IUserRepository>();
            _createTodoListUseCaseMock = new Mock<ICreateTodoListUseCase>();
        }

        [Fact]
        public async Task Should_create_new_list_when_no_errors()
        {
            // Arrange
            var expectedId = new TodoListId();
            const string listName = "todoList";
            var owner = UserFakeData.CreateUser();
            MockSuccessfulUseCaseResponse(expectedId, owner, listName);
            // act
            var response = await SendCreateTodoListCommand(owner.Email, listName);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var body = await response.Content.ReadAsStringAsync();
            body.Should().Be(expectedId.Value.ToString());
        }

        [Fact]
        public async Task Should_return_error_when_use_case_fails()
        {
            // arrange
            var error = new Error("TestingErrorKey", "ErrorMessage");
            const string listName = "todoList";
            var owner = UserFakeData.CreateUser();
            var expectedErrorResponse = MockFailUseCaseResponse(error, owner, listName);

            // act
            var response = await SendCreateTodoListCommand(owner.Email, listName);
            // assert
            await ErrorAssertionUtils.AssertError(response, expectedErrorResponse);
        }

        private async Task<HttpResponseMessage> SendCreateTodoListCommand(EmailAddress ownerEmailAddress,
            string todoListName)
        {
            var client = ConfigureClient();
            RestCreateTodoListRequest createTodoListRequest = new(todoListName);
            var request = new HttpRequestMessage(HttpMethod.Post, ControllerTestingConstants.TodoListPath);

            request.Content = HttpRequestHelper.GetStringContent(createTodoListRequest);
            request.Headers.Authorization = HttpRequestHelper.GetToken(ownerEmailAddress, new List<string>
            {
                Scopes.CreateTodoListsScope
            });

            var response = await client.SendAsync(request);
            return response;
        }

        private HttpClient ConfigureClient()
        {
            return _factory.WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddScoped(_ => _createTodoListUseCaseMock.Object);
                        services.AddScoped(_ => _userRepository.Object);
                    });
                })
                .CreateClient();
        }

        private RestErrorResponse MockFailUseCaseResponse(Error error, User owner, string listName)
        {
            var expectedErrorResponse = new RestErrorResponse((int) HttpStatusCode.InternalServerError,
                error.ErrorKey, error.Message);

            _userRepository.Setup(m => m.GetByEmail(owner.Email))
                .ReturnsAsync(owner);

            _createTodoListUseCaseMock.Setup(m =>
                    m.Invoke(It.Is(Match(owner.Id, listName))))
                .ThrowsAsync(new DomainException(error));

            _createTodoListUseCaseMock.Setup(m =>
                m.InvokeWithErrors(It.Is(Match(owner.Id, listName)))).ReturnsAsync(Left(error));
            return expectedErrorResponse;
        }

        private void MockSuccessfulUseCaseResponse(TodoListId expectedId, User owner, string listName)
        {
            _userRepository.Setup(m => m.GetByEmail(owner.Email))
                .ReturnsAsync(owner);

            _createTodoListUseCaseMock.Setup(m =>
                    m.Invoke(It.Is(Match(owner.Id, listName))))
                .ReturnsAsync(expectedId);

            _createTodoListUseCaseMock.Setup(m =>
                    m.InvokeWithErrors(It.Is(Match(owner.Id, listName))))
                .ReturnsAsync(Right(expectedId));
        }


        private static Expression<Func<CreateTodoListCommand, bool>> Match(UserId ownerId, string listName)
        {
            return c => listName.Equals(c.TodoListName.Name) && ownerId.Equals(c.OwnerId);
        }
    }
}