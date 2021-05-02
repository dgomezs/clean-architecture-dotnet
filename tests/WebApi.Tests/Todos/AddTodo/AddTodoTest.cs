using System;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Services.Todos.UseCases.AddTodo;
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
using WebApi.Controllers.Todos.TodoList;
using Xunit;
using static LanguageExt.Prelude;

namespace CleanArchitecture.TodoList.WebApi.Tests.Todos.AddTodo
{
    public class AddTodoTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly Mock<IAddTodoUseCase> _addTodoUseCase;
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public AddTodoTest(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _addTodoUseCase = new Mock<IAddTodoUseCase>();
        }


        private static Expression<Func<CreateTodoListCommand, bool>> Match(UserId ownerId, string listName)
        {
            return c => listName.Equals(c.TodoListName.Name) && ownerId.Equals(c.OwnerId);
        }
    }
}