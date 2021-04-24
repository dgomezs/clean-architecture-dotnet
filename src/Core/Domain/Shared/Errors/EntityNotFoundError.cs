namespace Domain.Shared.Errors
{
    public record EntityNotFoundError : Error
    {
        public EntityNotFoundError(string errorKey, string message) : base(errorKey, message)
        {
        }
    }
}