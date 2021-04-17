using System;
using Domain.Users.ValueObjects;

namespace Domain.Users
{
    public class User
    {
        public User(EmailAddress email)
        {
            Email = email;
            Id = new UserId(Guid.NewGuid());
        }

        public User(UserId userId, EmailAddress userEmail)
        {
            Id = userId;
            Email = userEmail;
        }

        public EmailAddress Email { get; }
        public UserId Id { get; }
    }
}