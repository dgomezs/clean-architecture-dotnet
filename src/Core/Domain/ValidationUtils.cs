﻿using System;
using Domain.Errors;
using FluentValidation;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Domain
{
    public static class ValidationUtils
    {
        public static Validation<DomainValidationException, T> WrapValidation<T>(Func<T> f, string errorKey)
        {
            return Try(f).ToValidation(ex => MapException(ex, errorKey));
        }

        private static DomainValidationException MapException(Exception ex, string errorKey)
        {
            return ex switch
            {
                ValidationException validationException => new DomainValidationException(errorKey,
                    validationException),
                _ => new DomainValidationException(errorKey,
                    new ValidationException($"Unknown validation error {ex.Message}"))
            };
        }
    }
}