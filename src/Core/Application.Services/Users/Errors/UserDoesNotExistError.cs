using System.Collections;
using System.Collections.Generic;
using Application.Services.Shared.Errors;
using Domain.Users.ValueObjects;

namespace Application.Services.Users.Errors
{
    public record UserDoesNotExistError : EntityAlreadyExistsError
    {
        public const string UserDoesNotExists = "UserDoesNotExists";
        private UserId _userId;

        public UserDoesNotExistError(UserId userId) : base(UserDoesNotExists,
            "User already exists") =>
            _userId = userId;

        public override IDictionary Data => new Dictionary<string, string>
        {
            {"UserId", _userId.Value.ToString()}
        };
    }
}