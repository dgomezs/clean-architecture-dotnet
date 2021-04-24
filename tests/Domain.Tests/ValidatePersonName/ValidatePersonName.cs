using System.Linq;
using Domain.Shared.Errors;
using Domain.Users.ValueObjects;
using Xunit;

namespace Domain.Tests.ValidatePersonName
{
    public class ValidatePersonName
    {
        private const string ValidLastName = "Smith";
        private const string ValidFirstName = "John";

        [Theory]
        [InlineData("Peter", "Smith")]
        public void Should_create_a_person_name_when_first_last_name_valid(string firstName, string lastName)
        {
            // act
            var personName = PersonName.Create(firstName, lastName);
            // assert
            Assert.Equal(personName.FirstName, firstName);
            Assert.Equal(personName.LastName, lastName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_throw_error_when_first_name_is_not_valid(string firstName)
        {
            var exception = Assert.Throws<DomainException>(() => PersonName.Create(firstName, ValidLastName));
            Assert.Equal(ErrorCodes.InvalidPersonName, exception.ErrorKey);
            Assert.Single(exception.Errors);
            var error = exception.Errors.Single();
            Assert.Equal(nameof(PersonName.FirstName), error.PropertyName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_throw_error_when_last_name_is_not_valid(string lastName)
        {
            var exception = Assert.Throws<DomainException>(() => PersonName.Create(ValidFirstName, lastName));
            Assert.Equal(ErrorCodes.InvalidPersonName, exception.ErrorKey);
            Assert.Single(exception.Errors);
            var error = exception.Errors.Single();
            Assert.Equal(nameof(PersonName.LastName), error.PropertyName);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("", null)]
        [InlineData(null, "")]
        [InlineData(null, null)]
        public void Should_throw_error_when_both_first_last_names_are_not_valid(string firstName,
            string lastName)
        {
            var exception = Assert.Throws<DomainException>(() => PersonName.Create(firstName, lastName));
            Assert.Equal(ErrorCodes.InvalidPersonName, exception.ErrorKey);
            Assert.Equal(2, exception.Errors.Count());
            var firstNameError = exception.Errors.First();
            var lastNameError = exception.Errors.Last();
            Assert.Equal(nameof(PersonName.FirstName), firstNameError.PropertyName);
            Assert.Equal(nameof(PersonName.LastName), lastNameError.PropertyName);
        }

        [Theory]
        [InlineData(" John ", "John", " J. Smith", "J. Smith")]
        public void Should_trim_first_and_last_names(string firstName, string expectedFirstName, string lastName,
            string expectedLastName)
        {
            // act
            var personName = PersonName.Create(firstName, lastName);
            // assert
            Assert.Equal(expectedFirstName, personName.FirstName);
            Assert.Equal(expectedLastName, personName.LastName);
        }
    }
}