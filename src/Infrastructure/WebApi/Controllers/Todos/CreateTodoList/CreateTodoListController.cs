using System;
using System.Threading.Tasks;
using Application.Services.Todos.UseCases.CreateTodoList;
using Ardalis.GuardClauses;
using Domain.Todos.ValueObjects;
using Domain.Users.ValueObjects;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers.Todos.CreateTodoList;
using static Application.Services.Shared.Extensions.EitherExtensions;

namespace WebApi.Controllers.CreateTodoList
{
    [Route("/todo-lists")]
    [ApiController]
    public class CreateTodoListController
    {
        private readonly ICreateTodoListUseCase _createTodoListUseCase;

        public CreateTodoListController(
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
