using Domain;
using Domain.Errors;
using Domain.ValueObjects;
using FluentValidation;
using LanguageExt;
using ErrorCodes = Application.Services.Errors.ErrorCodes;

namespace Application.Services.UseCases.CreateTodoList
{
    public class CreateTodoListRequest
    {
        public TodoListName TodoListName { get; }

        private CreateTodoListRequest(TodoListName todoListName) =>
            TodoListName = todoListName;

        public static CreateTodoListRequest Create(TodoListName todoListName)
        {
            var result = new CreateTodoListRequest(todoListName);
            var validator = new CreateTodoListRequestValidator();
            validator.ValidateAndThrow(result);
            return result;
        }

        public static Validation<DomainValidationException, CreateTodoListRequest> CreateWithErrors(
            TodoListName todoListName)
        {
            return ValidationUtils.WrapValidation(() => Create(todoListName),
                ErrorCodes.BadCreateTodoListRequest);
        }
    }

    internal class CreateTodoListRequestValidator : AbstractValidator<CreateTodoListRequest>
    {
        public CreateTodoListRequestValidator()
        {
            RuleFor(n => n.TodoListName)
                .NotNull();
        }
    }
}