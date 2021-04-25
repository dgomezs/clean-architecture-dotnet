using System.Collections;
using System.Collections.Generic;
using Domain.Shared.Errors;
using Domain.Users.ValueObjects;

namespace Domain.Todos.Errors
{
    public record UserNotAllowedToAddTodoError : Error
    {
        public const string UserNotAllowedToAddTodo = "UserNotAllowedToAddTodo";
        private readonly UserId _userId;

        public UserNotAllowedToAddTodoError(UserId userId) : base(UserNotAllowedToAddTodo,
            "User not allowed to add Todo") =>
            _userId = userId;

        public override IDictionary Data => new Dictionary<string, string>
        {
            {"UserId", _userId.Value.ToString()}
        };
    }
}