using System.Threading.Tasks;
using Application.Services.Todos.UseCases.CreateTodoList;
using Domain.Todos.ValueObjects;
using Domain.Users.ValueObjects;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
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
            [FromBody] RestCreateTodoListRequest createTodoListRequest)
        {
            var validator = new RestCreateTodoListRequestValidator();
            await validator.ValidateAndThrowAsync(createTodoListRequest);
            // TODO add owner id from authentication
            TodoListId result = await _createTodoListUseCase.InvokeWithErrors(
                CreateTodoListCommand.Create(new UserId(), createTodoListRequest.Name)).ToThrowException();
            return result.Value.ToString();
        }
    }
}