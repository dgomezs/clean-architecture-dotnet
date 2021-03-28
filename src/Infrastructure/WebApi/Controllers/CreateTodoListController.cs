using System.Threading.Tasks;
using Application.Services.UseCases.CreateTodoList;
using Domain.ValueObjects;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
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
            var result = await _createTodoListUseCase.Invoke(
                CreateTodoListCommand.Create(createTodoListRequest.Name));
            return result.Value.ToString();
        }

        [HttpGet("test")]
        public long CreateTodoListTest()
        {
            var validator = new RestCreateTodoListRequestValidator();
            return 1;
        }
    }

    public class RestCreateTodoListRequest
    {
        public RestCreateTodoListRequest(string name) =>
            Name = name;

        public string Name { get; }
    }

    public class RestCreateTodoListRequestValidator : AbstractValidator<RestCreateTodoListRequest>
    {
        public RestCreateTodoListRequestValidator()
        {
            var validator = new TodoListNameValidator();
            RuleFor(r => r.Name).SetValidator(validator).NotNull();
        }
    }
}