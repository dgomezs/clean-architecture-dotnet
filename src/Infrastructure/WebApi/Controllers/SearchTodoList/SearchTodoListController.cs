using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Services.UseCases.SearchTodoListByName;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.SearchTodoList
{
    [Route("/todo-lists")]
    [ApiController]
    public class SearchTodoListController
    {
        private readonly ISearchByNameTodoListUseCase _searchByNameTodoListUseCase;

        public SearchTodoListController(
            ISearchByNameTodoListUseCase searchByNameTodoListUseCase) =>
            _searchByNameTodoListUseCase = searchByNameTodoListUseCase;


        [HttpGet("search/by-name")]
        public async Task<List<TodoListReadModel>> SearchByName(
            [FromQuery] string name)
        {
            var validator = new SearchByNameValidator();
            await validator.ValidateAndThrowAsync(name);
            return await _searchByNameTodoListUseCase.SearchByName(name);
        }
    }

    public class SearchByNameValidator : AbstractValidator<string>
    {
        public SearchByNameValidator()
        {
            RuleFor(r => r)
                .NotEmpty()
                .NotNull();
        }
    }
}