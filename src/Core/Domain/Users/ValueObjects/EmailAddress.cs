using System;
using Domain.Shared.Errors;
using FluentValidation;

namespace Domain.Users.ValueObjects
{
    public record EmailAddress
    {
        private EmailAddress(string email) =>
            Email = email;

        public string Email { get; }

        public static EmailAddress Create(string email)
        {
            try
            {
                var validator = new EmailAddressValidator();
                validator.ValidateAndThrow(email);
                return new EmailAddress(email);
            }
            catch (Exception ex)
            {
                throw ValidationUtils.MapException(ex, ErrorCodes.InvalidEmailAddress);
            }
        }
    }

    public class EmailAddressValidator : AbstractValidator<string>
    {
        public EmailAddressValidator()
        {
            RuleFor(n => n)
                .EmailAddress().WithName("Email");
        }
    }
}