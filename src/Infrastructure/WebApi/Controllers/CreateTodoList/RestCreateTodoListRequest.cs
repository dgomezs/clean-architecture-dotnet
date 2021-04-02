using Domain.ValueObjects;
using FluentValidation;

namespace WebApi.Controllers.CreateTodoList
{
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