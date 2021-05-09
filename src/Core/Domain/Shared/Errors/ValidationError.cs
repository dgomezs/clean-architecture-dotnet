using FluentValidation.Results;

namespace Domain.Shared.Errors
{
    public record ValidationError : Error
    {
        public ValidationError(string code, string message) : base(code, message)
        {
        }

        public ValidationError(string code) : base(code)
        {
        }

        public ValidationError(ValidationFailure failure) : base(failure.ErrorCode, failure.PropertyName,
            failure.ErrorMessage)
        {
        }
    }
}