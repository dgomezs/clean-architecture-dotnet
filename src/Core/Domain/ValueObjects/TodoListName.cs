using System;
using Domain.Errors;
using FluentValidation;
using LanguageExt;

namespace Domain.ValueObjects
{
    public record TodoListName
    {
        public string Name { get; }
        private TodoListName(string name) => Name = name;

        public static TodoListName Create(string name)
        {
            try
            {
                var result = new TodoListName(name);
                var validator = new TodoListNameValidator();
                validator.ValidateAndThrow(result.Name);
                return result;
            }
            catch (Exception ex)
            {
                throw ValidationUtils.MapException(ex, ErrorCodes.InvalidTodoListName);
            }
        }

        public static Validation<DomainValidationException, TodoListName> CreateWithErrors(string name)
        {
            return ValidationUtils.WrapValidation(() => Create(name), ErrorCodes.InvalidTodoListName);
        }
    }

    public class TodoListNameValidator : AbstractValidator<string>
    {
        public TodoListNameValidator()
        {
            const int maximumLength = 50;
            RuleFor(n => n)
                .NotNull()
                .MinimumLength(1)
                .MaximumLength(maximumLength).WithName("TodoListName");
        }
    }
}