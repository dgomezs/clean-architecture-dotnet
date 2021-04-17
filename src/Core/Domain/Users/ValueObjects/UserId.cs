using System;
using Domain.Shared.ValueObjects;

namespace Domain.Users.ValueObjects
{
    public record UserId : GuidId
    {
        public UserId(Guid value) : base(value)
        {
        }
    }
}