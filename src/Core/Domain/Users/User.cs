using System;
using Domain.Shared.Entities;
using Domain.Users.ValueObjects;

namespace Domain.Users
{
    public class User : Entity
    {
        public User(EmailAddress email, PersonName name)
        {
            Email = email;
            Name = name;
            Id = new UserId(Guid.NewGuid());
        }

        public User(UserId userId, EmailAddress userEmail, PersonName name)
        {
            Id = userId;
            Email = userEmail;
            Name = name;
        }

        public EmailAddress Email { get; }
        public UserId Id { get; }
        public PersonName Name { get; }
    }
}