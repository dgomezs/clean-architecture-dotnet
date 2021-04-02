namespace Domain.Errors
{
    public record Error
    {
        public Error(string errorKey, string propertyName, string message)
        {
            ErrorKey = errorKey;
            PropertyName = propertyName;
            Message = message;
        }

        public Error(string errorKey, string message)
        {
            ErrorKey = errorKey;
            Message = message;
            PropertyName = null;
        }

        public Error(string errorKey)
        {
            ErrorKey = errorKey;
            Message = "";
            PropertyName = null;
        }


        public string ErrorKey { get; }
        public string? PropertyName { get; init; }
        public string Message { get; }
    }
}