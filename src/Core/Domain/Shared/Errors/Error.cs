using System.Collections;
using Ardalis.GuardClauses;

namespace Domain.Shared.Errors
{
    public record Error
    {
        public Error(string code, string propertyName, string message)
        {
            Code = Guard.Against.NullOrEmpty(code, nameof(code));
            PropertyName = Guard.Against.NullOrEmpty(propertyName, nameof(propertyName));
            Message = Guard.Against.NullOrEmpty(message, nameof(message));
        }

        public Error(string code, string message)
        {
            Code = Guard.Against.NullOrEmpty(code, nameof(code));
            Message = Guard.Against.NullOrEmpty(message, nameof(message));
            PropertyName = null;
        }

        public Error(string code)
        {
            Code = Guard.Against.NullOrEmpty(code, nameof(code));
            Message = "";
            PropertyName = null;
        }


        public string Code { get; }
        public string? PropertyName { get; init; }
        public string Message { get; }
        public virtual IDictionary Data { get; }
    }
}