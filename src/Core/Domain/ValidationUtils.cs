using System;
using Domain.Errors;
using FluentValidation;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Domain
{
    public static class ValidationUtils
    {
        public static Validation<DomainException, T> WrapValidation<T>(Func<T> f, string errorKey)
        {
            return Try(f).ToValidation(ex => MapException(ex, errorKey));
        }

        public static DomainException MapException(Exception ex, string errorKey)
        {
            return ex switch
            {
                ValidationException validationException => new DomainException(errorKey,
                    validationException.Errors),
                _ => new DomainException(new ValidationError(errorKey,
                    $"Unknown validation error {ex.Message}"))
            };
        }
    }
}