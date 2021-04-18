using System;
using Domain.Shared.Errors;
using FluentValidation;

namespace Domain.Users.ValueObjects
{
    public record EmailAddress
    {
        private EmailAddress(string email) =>
            Value = email.ToLower().Trim();

        public string Value { get; }

        public static EmailAddress Create(string? email)
        {
            try
            {
                var result = new EmailAddress(email ?? "");
                var validator = new EmailAddressValidator();
                validator.ValidateAndThrow(result.Value);
                return result;
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