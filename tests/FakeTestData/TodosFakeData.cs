using System.Collections.Generic;
using System.Linq;
using Bogus;
using Domain.Todos.Entities;
using Domain.Todos.ValueObjects;

namespace FakeTestData
{
    public static class TodosFakeData
    {
        public static Todo CreateTodoNotDone()
        {
            return CreateTodosNotDone(1).First();
        }

        public static List<Todo> CreateTodosNotDone(int count)
        {
            var todoFaker = TodoFaker();
            return Enumerable.Range(0, count).Select(_ => todoFaker.Generate()).ToList();
        }

        public static TodoDescription CreateTodoDescription()
        {
            var generator = new Faker();
            var todoDescription = generator.Random.AlphaNumeric(5);
            return TodoDescription.Create(todoDescription);
        }

        private static Faker<Todo> TodoFaker()
        {
            return new Faker<Todo>()
                .CustomInstantiator(f => new Todo(TodoDescription.Create(f.Random.AlphaNumeric(30))));
        }
    }
}