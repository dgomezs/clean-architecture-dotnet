using Domain.Errors;

namespace Application.Services.Errors
{
    public record EntityDoesNotExistError : Error
    {
        public EntityDoesNotExistError(string errorKey, string propertyName, string message) : base(errorKey,
            propertyName, message)
        {
        }

        public EntityDoesNotExistError(string errorKey, string message) : base(errorKey, message)
        {
        }

        public EntityDoesNotExistError(string errorKey) : base(errorKey)
        {
        }
    }
}