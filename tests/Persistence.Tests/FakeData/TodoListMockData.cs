using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Domain.Entities;
using Domain.ValueObjects;

namespace Persistence.Tests.FakeData
{
    public static class TodoListMockData
    {
        public static TodoList CreateTodoList(int numberOfTodos = 0)
        {
            return CreateTodoList("", 1, numberOfTodos).First();
        }

        public static List<TodoList> CreateTodoList(string namePrefix, int count, int numberOfTodos = 0)
        {
            var todoListName = TodoListNameFaker(namePrefix);
            return Enumerable.Range(0, count).Select(_ => new TodoList(todoListName.Generate(),
                new TodoListId(Guid.NewGuid()), CreateTodos(numberOfTodos))).ToList();
        }

        private static Faker<TodoListName> TodoListNameFaker(string prefix)
        {
            return new Faker<TodoListName>()
                .CustomInstantiator(f => TodoListName.Create(prefix + f.Random.AlphaNumeric(30)));
        }

        private static Faker<Todo> TodoFaker()
        {
            return new Faker<Todo>()
                .CustomInstantiator(f => new Todo(TodoDescription.Create(f.Random.AlphaNumeric(30))));
        }

        private static List<Todo> CreateTodos(int count)
        {
            var todoFaker = TodoFaker();
            return Enumerable.Range(0, count).Select(_ => todoFaker.Generate()).ToList();
        }
    }
}