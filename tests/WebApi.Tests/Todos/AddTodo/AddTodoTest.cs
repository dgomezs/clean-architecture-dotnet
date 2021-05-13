using System;
using System.Linq.Expressions;
using Application.Services.Todos.UseCases.AddTodo;
using Application.Services.Todos.UseCases.CreateTodoList;
using CleanArchitecture.TodoList.WebApi.Tests.Config;
using Domain.Users.ValueObjects;
using Moq;
using Xunit;

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