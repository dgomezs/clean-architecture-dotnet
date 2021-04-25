using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Services.Todos.UseCases.SearchTodoListByName;
using Ardalis.GuardClauses;
using Domain.Users.ValueObjects;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Todos.SearchTodoList
{
    [Route("/todo-lists")]
    [ApiController]
    public class SearchTodoListController
    {
        [HttpGet("search/by-name")]
        public async Task<List<TodoListReadModel>> SearchByName(
            [FromServices] ISearchByNameTodoListUseCase searchByNameTodoListUseCase,
            [FromHeader(Name = "OwnerId")] string ownerIdValue,
            [FromQuery] string? name)
        {
            var ownerId = ValidateOwnerId(ownerIdValue);
            var nameValue = await ValidateName(name);
            return await searchByNameTodoListUseCase.SearchByName(ownerId, nameValue);
        }

        private static UserId ValidateOwnerId(string ownerIdValue)
        {
            return new UserId(new Guid(Guard.Against.NullOrEmpty(ownerIdValue, nameof(ownerIdValue))));
        }

        private static async Task<string> ValidateName(string? name)
        {
            var validator = new SearchByNameValidator();
            var nameValue = name ?? "";
            await validator.ValidateAndThrowAsync(nameValue);
            return nameValue;
        }
    }

    public class SearchByNameValidator : AbstractValidator<string>
    {
        public SearchByNameValidator()
        {
            RuleFor(r => r)
                .NotEmpty()
                .NotNull().WithName("Name").WithMessage("Invalid name");
        }
    }
}