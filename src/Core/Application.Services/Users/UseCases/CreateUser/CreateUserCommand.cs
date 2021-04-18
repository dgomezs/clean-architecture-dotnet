using Domain.Users.ValueObjects;

namespace Application.Services.Users.UseCases.CreateUser
{
    public record CreateUserCommand
    {
        private CreateUserCommand(PersonName name, EmailAddress email)
        {
            Name = name;
            Email = email;
        }

        public EmailAddress Email { get; }

        public PersonName Name { get; }


        public static CreateUserCommand Create(string firstName, string lastName, string email)
        {
            return new(PersonName.Create(firstName, lastName), EmailAddress.Create(email));
        }
    }
}