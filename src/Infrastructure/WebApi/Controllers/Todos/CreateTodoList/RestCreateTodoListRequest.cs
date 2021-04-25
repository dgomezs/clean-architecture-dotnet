using Domain.Todos.ValueObjects;
using FluentValidation;

namespace WebApi.Controllers.CreateTodoList
{
    public record RestCreateTodoListRequest (string Name)
    {
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