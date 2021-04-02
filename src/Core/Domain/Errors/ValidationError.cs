using FluentValidation.Results;
using JetBrains.Annotations;

namespace Domain.Errors
{
    public record ValidationError : Error
    {
        public ValidationError(string errorKey, string propertyName, string message) : base(errorKey,
            propertyName, message)
        {
        }

        public ValidationError(string errorKey, string message) : base(errorKey, message)
        {
        }

        public ValidationError(string errorKey) : base(errorKey)
        {
        }

        public ValidationError(ValidationFailure failure) : base(failure.ErrorCode, failure.PropertyName,
            failure.ErrorMessage)
        {
        }
    }
}