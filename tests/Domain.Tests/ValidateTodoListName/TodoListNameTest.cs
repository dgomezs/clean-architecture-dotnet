using Domain.Errors;
using Domain.ValueObjects;
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
            var resultListName = TodoListName.CreateWithErrors(validListName);
            // assert
            Assert.Equal(validListName, listName.Name);
            resultListName.Match(
                n => Assert.Equal(validListName, n.Name),
                ex => throw ex[0]);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("this-is-a-very-long-validation-name-that-is-not-accepted")]
        public void Should_throw_a_validation_exception_for_invalid_names(string invalidName)
        {
            var domainValidationException =
                Assert.Throws<DomainValidationException>(() => TodoListName.Create(invalidName));
            Assert.True(domainValidationException.ErrorKey.Equals(ErrorCodes.InvalidTodoListName));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("this-is-a-very-long-validation-name-that-is-not-accepted")]
        public void Should_return_a_validation_exception_for_invalid_names(string invalidName)
        {
            ;
            var result = TodoListName.CreateWithErrors(invalidName);
            Assert.True(result.IsFail);
            Assert.True(result.MapFail(r => r.ErrorKey).Equals(ErrorCodes.InvalidTodoListName));
        }
    }
}