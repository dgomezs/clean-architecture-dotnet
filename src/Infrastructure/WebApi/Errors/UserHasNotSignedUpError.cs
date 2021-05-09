using System.Collections;
using System.Collections.Generic;
using Application.Services.Shared.Errors;
using Domain.Shared.Errors;
using Domain.Users.ValueObjects;

namespace WebApi.Errors
{
    public record UserHasNotSignedUpError : EntityNotFoundError
    {
        public const string UserHasNotSignedUp = "UserHasNotSignedUp";
        private readonly string _email;

        public UserHasNotSignedUpError(EmailAddress email) : base(UserHasNotSignedUp,
            "User has not signed up") =>
            _email = email.Value;

        public override IDictionary Data => new Dictionary<string, string>
        {
            {"Email", _email}
        };
    }
}