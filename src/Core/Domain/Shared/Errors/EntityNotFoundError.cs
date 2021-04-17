namespace Domain.Shared.Errors
{
    public record EntityNotFoundError : Error
    {
        public EntityNotFoundError(string errorKey, string propertyName, string message) : base(errorKey,
            propertyName, message)
        {
        }

        public EntityNotFoundError(string errorKey, string message) : base(errorKey, message)
        {
        }

        public EntityNotFoundError(string errorKey) : base(errorKey)
        {
        }
    }
}