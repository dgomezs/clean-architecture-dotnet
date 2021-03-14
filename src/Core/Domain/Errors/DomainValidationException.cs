using System;
using FluentValidation;

namespace Domain.Errors
{
    public class DomainValidationException : DomainException
    {
        public ValidationException InternalValidationException { get; }

        public DomainValidationException(string errorKey, ValidationException ex) : base(errorKey) =>
            InternalValidationException = ex;

        public DomainValidationException(string errorKey, string errorMessage, ValidationException ex) : base(errorKey, errorMessage) =>
            InternalValidationException = ex;
    }
}