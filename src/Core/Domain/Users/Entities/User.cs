using Ardalis.GuardClauses;
using Domain.Shared.Entities;
using Domain.Users.ValueObjects;

namespace Domain.Users.Entities
{
    public class User : Entity
    {
        public User(EmailAddress email, PersonName name)
        {
            Email = Guard.Against.Null(email, nameof(email));
            Name = Guard.Against.Null(name, nameof(name));
            Id = new UserId();
        }

        public User(UserId userId, EmailAddress email, PersonName name)
        {
            Id = Guard.Against.Null(userId, nameof(userId));
            Email = Guard.Against.Null(email, nameof(email));
            Name = Guard.Against.Null(name, nameof(name));
        }

        public EmailAddress Email { get; }
        public UserId Id { get; }
        public PersonName Name { get; }
    }
}