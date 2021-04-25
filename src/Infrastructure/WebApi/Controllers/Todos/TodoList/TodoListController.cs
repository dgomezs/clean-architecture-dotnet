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
        [HttpPost]
        public async Task<string> CreateTodoList(
            [FromServices] ICreateTodoListUseCase createTodoListUseCase,
            [FromHeader(Name = "OwnerId")] string ownerIdValue,
            [FromBody] RestCreateTodoListRequest createTodoListRequest)
        {
            var ownerId = new UserId(new Guid(Guard.Against.NullOrEmpty(ownerIdValue, nameof(ownerIdValue))));

            // TODO add owner id from authentication
            var createTodoListCommand = CreateTodoListCommand.Create(ownerId, createTodoListRequest.Name);
            TodoListId result = await createTodoListUseCase.InvokeWithErrors(
                    createTodoListCommand)
                .ToThrowException();
            return result.Value.ToString();
        }
    }
}