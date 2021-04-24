using Ardalis.GuardClauses;
using Domain.Shared.Entities;
using Domain.Users.ValueObjects;

namespace Domain.Users.Entities
{
    public class User : Entity
    {
        public User(PersonName name, EmailAddress email)
        {
            Email = Guard.Against.Null(email, nameof(email));
            Name = Guard.Against.Null(name, nameof(name));
            Id = new UserId();
        }

        public User(UserId userId, PersonName name, EmailAddress email)
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