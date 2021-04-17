using System.Collections;
using System.Collections.Generic;
using Domain.Users.ValueObjects;

namespace Application.Services.Errors.Users
{
    public record UserAlreadyExistsError : EntityAlreadyExistsError
    {
        public const string UserAlreadyExists = "UserAlreadyExists";
        private readonly EmailAddress _email;

        public UserAlreadyExistsError(EmailAddress email) : base(UserAlreadyExists,
            "User already exists") =>
            _email = email;

        public override IDictionary Data => new Dictionary<string, string>
        {
            {"Email", _email.Email}
        };
    }
}