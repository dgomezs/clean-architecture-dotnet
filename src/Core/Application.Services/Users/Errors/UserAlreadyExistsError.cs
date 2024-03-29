﻿using System.Collections;
using System.Collections.Generic;
using Application.Services.Shared.Errors;
using Domain.Users.ValueObjects;

namespace Application.Services.Users.Errors
{
    public record UserAlreadyExistsError : EntityAlreadyExistsError
    {
        public const string UserAlreadyExists = "UserAlreadyExists";
        private readonly string _email;

        public UserAlreadyExistsError(EmailAddress email) : base(UserAlreadyExists,
            "User already exists") =>
            _email = email.Value;

        public override IDictionary Data => new Dictionary<string, string>
        {
            {"Email", _email}
        };
    }
}