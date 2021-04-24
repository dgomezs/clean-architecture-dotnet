using System;
using Domain.Shared.Errors;
using FluentValidation;

namespace Domain.Users.ValueObjects
{
    public record PersonName
    {
        private PersonName(string firstName, string lastName)
        {
            FirstName = firstName.Trim();
            LastName = lastName.Trim();
        }

        public string LastName { get; }

        public string FirstName { get; }

        public static PersonName Create(string? firstName, string? lastName)
        {
            try
            {
                var result = new PersonName(firstName ?? "", lastName ?? "");
                var validator = new PersonNameValidator();
                validator.ValidateAndThrow(result);
                return result;
            }
            catch (Exception e)
            {
                throw ValidationUtils.MapException(e, ErrorCodes.InvalidPersonName);
            }
        }
    }

    public class PersonNameValidator : AbstractValidator<PersonName>
    {
        public PersonNameValidator()
        {
            RuleFor(n => n.FirstName)
                .SetValidator(new FirstNameValidator());

            RuleFor(n => n.LastName)
                .SetValidator(new LastNameValidator());
        }
    }

    public class FirstNameValidator : AbstractValidator<string>
    {
        public FirstNameValidator()
        {
            RuleFor(n => n)
                .NotEmpty().WithName(nameof(PersonName.FirstName));
        }
    }

    public class LastNameValidator : AbstractValidator<string>
    {
        public LastNameValidator()
        {
            RuleFor(n => n)
                .NotEmpty().WithName(nameof(PersonName.LastName));
        }
    }
}