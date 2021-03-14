using Domain.ValueObjects;
using FluentValidation;
using Xunit;

namespace Domain.Tests.ValidateTodoListName
{
    public class TodoListNameTest
    {
        [Fact]
        public void Should_create_a_todo_list_name_when_valid_name()
        {
            // arrange
            var validListName = "valid-list-name";
            // act
            var listName = TodoListName.Create(validListName);
            // assert
            Assert.Equal(validListName, listName.Name);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("this-is-a-very-long-validation-name-that-is-not-accepted")]
        public void Should_throw_a_validation_exception_for_invalid_names(string invalidName)
        {
            Assert.Throws<ValidationException>(() => TodoListName.Create(invalidName));
        }
    }
}