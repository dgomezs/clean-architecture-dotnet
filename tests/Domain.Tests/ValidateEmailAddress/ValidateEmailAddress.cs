using Domain.Shared.Errors;
using Domain.Users.ValueObjects;
using Xunit;

namespace Domain.Tests.ValidateEmailAddress
{
    public class ValidateEmailAddress
    {
        [Theory]
        [InlineData(
            "1234567890123456789012345678901234567890123456789012345678901234+x@example.com")]
        [InlineData("very.common@example.com")]
        [InlineData("abc@example.co.uk")]
        [InlineData("disposable.style.email.with+symbol@example.com")]
        [InlineData("other.email-with-hyphen@example.com")]
        [InlineData("fully-qualified-domain@example.com")]
        [InlineData("user.name+tag+sorting@example.com")]
        [InlineData("example-indeed@strange-example.com ")]
        [InlineData("example-indeed@strange-example.inininini")]
        [InlineData("_______@example.com")]
        public void Should_create_a_email_address_when_the_address_is_valid(string validEmailAddress)
        {
            // act
            var emailAddress = EmailAddress.Create(validEmailAddress);
            // assert
            Assert.Equal(validEmailAddress, emailAddress.Email);
        }

        [Theory]
        [InlineData("")]
        [InlineData("plainaddress")]
        [InlineData("#@%^%#$@#$@#.com")]
        [InlineData("@example.com")]
        public void Should_throw_an_error_when_the_address_is_invalid(string invalidEmailAddress)
        {
            // assert
            var exception = Assert.Throws<DomainException>(() => EmailAddress.Create(invalidEmailAddress));
            Assert.Equal(ErrorCodes.InvalidEmailAddress, exception.ErrorKey);
        }
    }
}