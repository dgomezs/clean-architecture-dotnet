using System;
using Domain.Errors;

namespace Application.Services.Errors
{
    public class EntityExistsException : DomainException
    {
        public EntityExistsException(string errorKey) : base(errorKey)
        {
        }

        public EntityExistsException(string errorKey, string errorMessage) : base(errorKey,
            errorMessage)
        {
        }

        public EntityExistsException(string errorKey, string errorMessage,
            Exception innerException) : base(errorKey, errorMessage, innerException)
        {
        }
    }
}