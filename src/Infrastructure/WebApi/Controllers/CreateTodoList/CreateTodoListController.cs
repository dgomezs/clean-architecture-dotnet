using System.Threading.Tasks;
using Application.Services.UseCases.CreateTodoList;
using Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using static Domain.Extensions.EitherExtensions;

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
            TodoListId result = await _createTodoListUseCase.InvokeWithErrors(
                CreateTodoListCommand.Create(createTodoListRequest.Name)).ToThrowException();
            return result.Value.ToString();
        }
    }
}