using Application.Services.UseCases.CreateTodoList;
using Bogus;

namespace Application.Services.Tests
{
    public static class MockDataGenerator
    {
        public static CreateTodoListCommand CreateTodoList()
        {
            var generator = new Faker();
            var todoListName = generator.Random.AlphaNumeric(5);
            var foo = CreateTodoListCommand.Create(todoListName);
            return foo;
        }
    }
}