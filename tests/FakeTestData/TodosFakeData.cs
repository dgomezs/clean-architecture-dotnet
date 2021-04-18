using System.Collections.Generic;
using System.Linq;
using Bogus;
using Domain.Todos.Entities;
using Domain.Todos.ValueObjects;

namespace FakeTestData
{
    public class TodosFakeData
    {
        public static Todo CreateTodoNotDone()
        {
            return new(CreateTodoDescription());
        }

        private static Faker<Todo> TodoFaker()
        {
            return new Faker<Todo>()
                .CustomInstantiator(f => new Todo(TodoDescription.Create(f.Random.AlphaNumeric(30))));
        }

        public static List<Todo> CreateTodos(int count)
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
    }
}