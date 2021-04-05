using Application.Services.UseCases.CreateTodoList;
using Bogus;
using Domain.ValueObjects;

namespace Application.Services.Tests
{
    public static class MockDataGenerator
    {
        public static CreateTodoListCommand CreateTodoList()
        {
            var generator = new Faker();
            var todoListName = generator.Random.AlphaNumeric(5);
            return CreateTodoListCommand.Create(todoListName);
        }

        public static TodoDescription CreateTodoDescription()
        {
            var generator = new Faker();
            var todoDescription = generator.Random.AlphaNumeric(5);
            return TodoDescription.Create(todoDescription);
        }
    }
}