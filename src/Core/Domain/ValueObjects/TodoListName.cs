using System.Collections.Generic;
using Domain.Errors;
using FluentValidation;
using LanguageExt;

namespace Domain.ValueObjects
{
    public class TodoListName : ValueObject
    {
        public string Name { get; }
        private TodoListName(string name) => Name = name;

        public static TodoListName Create(string name)
        {
            var result = new TodoListName(name);
            var validator = new TodoListNameValidator();
            validator.ValidateAndThrow(result);
            return result;
        }

        public static Validation<DomainValidationException, TodoListName> CreateWithErrors(string name)
        {
            return ValidationUtils.WrapValidation(() => Create(name), ErrorCodes.InvalidTodoListName);
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Name;
        }
    }

    internal class TodoListNameValidator : AbstractValidator<TodoListName>
    {
        public TodoListNameValidator()
        {
            const int maximumLength = 50;
            RuleFor(n => n.Name)
                .NotNull()
                .MinimumLength(1)
                .MaximumLength(maximumLength);
        }
    }
}