using System.Collections.Generic;
using System.Linq;
using Bogus;
using Domain.Entities;
using Domain.ValueObjects;

namespace Persistence.Tests.FakeData
{
    public class TodoListMockData
    {
        public static TodoList CreateTodoList()
        {
            ;
            return CreateTodoList("", 1).First();
        }

        public static List<TodoList> CreateTodoList(string namePrefix, int count)
        {
            var todoListName = TodoListNameFaker(namePrefix);
            return Enumerable.Range(0, count).Select(_ => new TodoList(todoListName.Generate())).ToList();
        }

        private static Faker<TodoListName> TodoListNameFaker(string prefix)
        {
            return new Faker<TodoListName>()
                .CustomInstantiator(f => TodoListName.Create(prefix + f.Random.AlphaNumeric(30)));
        }
    }
}