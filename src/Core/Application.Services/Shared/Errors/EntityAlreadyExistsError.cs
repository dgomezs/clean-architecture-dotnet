using Domain.Shared.Errors;

namespace Application.Services.Shared.Errors
{
    public record EntityAlreadyExistsError : Error
    {
        public EntityAlreadyExistsError(string errorKey, string propertyName, string message) : base(errorKey,
            propertyName, message)
        {
        }

        public EntityAlreadyExistsError(string errorKey, string message) : base(errorKey, message)
        {
        }

        public EntityAlreadyExistsError(string errorKey) : base(errorKey)
        {
        }
    }
}