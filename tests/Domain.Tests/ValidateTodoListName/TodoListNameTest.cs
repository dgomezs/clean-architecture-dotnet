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
            // assert
            Assert.Equal(validListName, listName.Name);
        }
    }
}