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
        public async Task<long> CreateTodoList(
            [FromBody] RestCreateTodoListRequest createTodoListRequest)
        {
            var validator = new RestCreateTodoListRequestValidator();
            await validator.ValidateAndThrowAsync(createTodoListRequest);
            return await _createTodoListUseCase.Invoke(
                CreateTodoListRequest.Create(createTodoListRequest.Name1, createTodoListRequest.Name2));
        }
    }

    public class RestCreateTodoListRequest
    {
        public string Name1 { get; set; }
        public string Name2 { get; set; }
    }

    public class RestCreateTodoListRequestValidator : AbstractValidator<RestCreateTodoListRequest>
    {
        public RestCreateTodoListRequestValidator()
        {
            var validator = new TodoListNameValidator();
            RuleFor(r => r.Name1).SetValidator(validator).NotNull();
            RuleFor(r => r.Name2).SetValidator(validator);
        }
    }
}