namespace Domain.Errors
{
    public record Error
    {
        public Error(string errorCode, string propertyName, string message)
        {
            ErrorCode = errorCode;
            PropertyName = propertyName;
            Message = message;
        }

        public Error(string errorCode, string message)
        {
            ErrorCode = errorCode;
            Message = message;
            PropertyName = null;
        }

        public Error(string errorCode)
        {
            ErrorCode = errorCode;
            Message = "";
            PropertyName = null;
        }


        public string ErrorCode { get; }
        public string? PropertyName { get; init; }
        public string Message { get; }
    }
}