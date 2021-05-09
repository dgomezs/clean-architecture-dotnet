namespace Domain.Shared.Errors
{
    public record EntityNotFoundError : Error
    {
        protected EntityNotFoundError(string code, string message) : base(code, message)
        {
        }
    }
}