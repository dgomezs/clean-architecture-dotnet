namespace Domain.Errors
{
    public record DomainValidationFailure (string ErrorCode, string PropertyName, string Message)
    {
        
    }
}