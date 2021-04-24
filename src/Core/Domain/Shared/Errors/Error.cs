using System.Collections;
using Ardalis.GuardClauses;

namespace Domain.Shared.Errors
{
    public record Error
    {
        public Error(string errorKey, string propertyName, string message)
        {
            ErrorKey = Guard.Against.NullOrEmpty(errorKey, nameof(errorKey));
            PropertyName = Guard.Against.NullOrEmpty(propertyName, nameof(propertyName));
            Message = Guard.Against.NullOrEmpty(message, nameof(message));
        }

        public Error(string errorKey, string message)
        {
            ErrorKey = Guard.Against.NullOrEmpty(errorKey, nameof(errorKey));
            Message = Guard.Against.NullOrEmpty(message, nameof(message));
            PropertyName = null;
        }

        public Error(string errorKey)
        {
            ErrorKey = Guard.Against.NullOrEmpty(errorKey, nameof(errorKey));
            Message = "";
            PropertyName = null;
        }


        public string ErrorKey { get; }
        public string? PropertyName { get; init; }
        public string Message { get; }
        public virtual IDictionary Data { get; }
    }
}