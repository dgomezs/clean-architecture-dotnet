using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Domain.Todos.Entities;
using Domain.Todos.ValueObjects;

namespace FakeTestData
{
    public static class TodoListFakeData
    {
        public static TodoList CreateTodoList(int numberOfTodos = 0, int numberOfTodosDone = 0)
        {
            var todoList = CreateTodoList("", 1, numberOfTodos)
                .First();

            var ids = todoList.Todos.ToList().GetRange(0, numberOfTodosDone).Select(_ => _.Id);
            foreach (var id in ids)
                todoList.MarkAsDone(id);

            return todoList;
        }

        public static TodoListName CreateTodoListName()
        {
            var generator = new Faker();
            var name = generator.Random.AlphaNumeric(5);
            return TodoListName.Create(name);
        }

        private static TodoListId CreateTodoListId()
        {
            return new(Guid.NewGuid());
        }


        public static List<TodoList> CreateTodoList(string namePrefix, int count, int numberOfTodos = 0)
        {
            var todoListName = TodoListNameFaker(namePrefix);
            return Enumerable.Range(0, count).Select(_ => new TodoList(todoListName.Generate(),
                new TodoListId(Guid.NewGuid()), TodosFakeData.CreateTodos(numberOfTodos))).ToList();
        }

        private static Faker<TodoListName> TodoListNameFaker(string prefix)
        {
            return new Faker<TodoListName>()
                .CustomInstantiator(f => TodoListName.Create(prefix + f.Random.AlphaNumeric(30)));
        }


        public static TodoList CreateTodoListWithNumberTodosNotDone(
            int numberOfTodosNotDone)
        {
            var todos = Enumerable.Range(0, numberOfTodosNotDone)
                .Select(x => TodosFakeData.CreateTodoNotDone()).ToList();

            return new TodoList(CreateTodoListName(), CreateTodoListId(), todos);
        }
    }
}