using Application.Services.UseCases.CreateTodoList;
using Bogus;
using Domain.ValueObjects;

namespace Application.Services.Tests
{
    public static class MockDataGenerator
    {
        public static CreateTodoListRequest CreateTodoList()
        {
            var generator = new Faker();
            var foo = CreateTodoListRequest.Create(TodoListName.Create(generator.Random.AlphaNumeric(5)));
            return foo;
        }
    }
}