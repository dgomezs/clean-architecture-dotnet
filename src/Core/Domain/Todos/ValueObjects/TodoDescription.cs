using System;
using Domain.Shared.Errors;
using FluentValidation;

namespace Domain.Todos.ValueObjects
{
    public record TodoDescription
    {
        private TodoDescription(string description) =>
            Description = description.Trim();

        public string Description { get; }

        public static TodoDescription Create(string description)
        {
            try
            {
                var result = new TodoDescription(description);
                var validator = new TodoDescriptionValidator();
                validator.ValidateAndThrow(result.Description);
                return result;
            }
            catch (Exception ex)
            {
                throw ValidationUtils.MapException(ex, ErrorCodes.InvalidTodoDescription);
            }
        }
    }

    public class TodoDescriptionValidator : AbstractValidator<string>
    {
        public TodoDescriptionValidator()
        {
            const int maximumLength = 250;
            RuleFor(n => n)
                .NotNull()
                .MinimumLength(1)
                .MaximumLength(maximumLength).WithName("TodoDescription");
        }
    }
}