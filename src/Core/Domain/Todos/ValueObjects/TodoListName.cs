using System;
using Domain.Shared.Errors;
using FluentValidation;
using LanguageExt;

namespace Domain.Todos.ValueObjects
{
    public record TodoListName
    {
        private TodoListName(string name) => Name = name.Trim();
        public string Name { get; }

        public static TodoListName Create(string? name)
        {
            try
            {
                var result = new TodoListName(name ?? "");
                var validator = new TodoListNameValidator();
                validator.ValidateAndThrow(result.Name);
                return result;
            }
            catch (Exception ex)
            {
                throw ValidationUtils.MapException(ex, ErrorCodes.InvalidTodoListName);
            }
        }

        public static Validation<DomainException, TodoListName> CreateWithErrors(string name)
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