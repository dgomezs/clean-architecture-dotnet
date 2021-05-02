using System.Collections;
using System.Collections.Generic;
using Application.Services.Shared.Errors;
using Domain.Users.ValueObjects;

namespace Application.Services.Users.Errors
{
    public record UserDoesNotExistError : EntityAlreadyExistsError
    {
        public const string UserDoesNotExists = "UserDoesNotExists";
        private readonly string _userId;
        private readonly string _userEmailAddress;

        public UserDoesNotExistError(UserId userId) : base(UserDoesNotExists,
            "User does not exist")
        {
            _userId = userId.Value.ToString();
            _userEmailAddress = string.Empty;
        }

        public UserDoesNotExistError(EmailAddress emailAddress) : base(UserDoesNotExists,
            "User does not exist")
        {
            _userId = string.Empty;
            _userEmailAddress = emailAddress.Value;
        }

        public override IDictionary Data => new Dictionary<string, string>
        {
            {"UserId", _userId},
            {"Email", _userEmailAddress},
        };
    }
}