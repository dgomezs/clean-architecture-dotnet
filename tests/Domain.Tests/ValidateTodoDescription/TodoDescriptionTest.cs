using Bogus;
using Domain.Shared.Errors;
using Domain.Todos.ValueObjects;
using Xunit;

namespace Domain.Tests.ValidateTodoDescription
{
    public class TodoDescriptionTest
    {
        [Fact]
        public void Should_create_a_todo_description_when_valid_name()
        {
            // arrange
            const string validTodoDescription = "valid-todo-description";
            // act
            var todoDescription = TodoDescription.Create(validTodoDescription);
            // assert
            Assert.Equal(validTodoDescription, todoDescription.Description);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_throw_a_validation_exception_for_empty_names(string invalidName)
        {
            AssertTodoDescriptionName(invalidName);
        }
        
        [Fact]
        public void Should_throw_a_validation_exception_for_very_long_descriptions()
        {
            // arrange
            var faker = new Faker();
            // act
            // assert
            AssertTodoDescriptionName(faker.Random.AlphaNumeric(251));
        }

        private static void AssertTodoDescriptionName(string invalidName)
        {
            var domainValidationException =
                Assert.Throws<DomainException>(() => TodoDescription.Create(invalidName));
            Assert.True(domainValidationException.ErrorKey.Equals(ErrorCodes.InvalidTodoDescription));
        }
    }
}