using Domain.Shared.Errors;

namespace Application.Services.Shared.Errors
{
    public record EntityAlreadyExistsError : Error
    {
        public EntityAlreadyExistsError(string code, string propertyName, string message) : base(code,
            propertyName, message)
        {
        }

        public EntityAlreadyExistsError(string code, string message) : base(code, message)
        {
        }

        public EntityAlreadyExistsError(string code) : base(code)
        {
        }
    }
}