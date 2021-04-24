using Bogus;
using Domain.Users.Entities;
using Domain.Users.ValueObjects;

namespace FakeTestData
{
    public static class UserFakeData
    {
        public static PersonName CreatePersonName()
        {
            var generator = new Faker();
            return PersonName.Create(generator.Person.FirstName,
                generator.Person.LastName);
        }

        public static EmailAddress CreateEmail()
        {
            var generator = new Faker();
            return EmailAddress.Create(generator.Person.Email);
        }

        public static User CreateUser()
        {
            return new User(CreatePersonName(), CreateEmail());
        }
    }
}