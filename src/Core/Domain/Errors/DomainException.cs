using System;

namespace Domain.Errors
{
    public class DomainException : Exception
    {
        public DomainException(string errorKey) =>
            ErrorKey = errorKey;

        public DomainException(string errorKey, string errorMessage) : base(errorMessage) =>
            ErrorKey = errorKey;

        public DomainException(string errorKey, string errorMessage, Exception innerException) : base(
            errorMessage, innerException) =>
            ErrorKey = errorKey;

        public string ErrorKey { get; }
    }
}