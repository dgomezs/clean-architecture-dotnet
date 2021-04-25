using System;
using System.Threading.Tasks;
using Application.Services.Todos.UseCases.CreateTodoList;
using Ardalis.GuardClauses;
using Domain.Todos.ValueObjects;
using Domain.Users.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using static Application.Services.Shared.Extensions.EitherExtensions;

namespace WebApi.Controllers.Todos.TodoList
{
    [Route("/todo-lists")]
    [ApiController]
    public class TodoListController
    {
        private readonly ICreateTodoListUseCase _createTodoListUseCase;

        public TodoListController(
            ICreateTodoListUseCase createTodoListUseCase) =>
            _createTodoListUseCase = createTodoListUseCase;


        [HttpPost]
        public async Task<string> CreateTodoList(
            [FromHeader(Name = "OwnerId")] string ownerIdValue,
            [FromBody] RestCreateTodoListRequest createTodoListRequest)
        {
            var ownerId = new UserId(new Guid(Guard.Against.NullOrEmpty(ownerIdValue, nameof(ownerIdValue))));

            // TODO add owner id from authentication
            TodoListId result = await _createTodoListUseCase.InvokeWithErrors(
                    CreateTodoListCommand.Create(ownerId, createTodoListRequest.Name))
                .ToThrowException();
            return result.Value.ToString();
        }
    }
}