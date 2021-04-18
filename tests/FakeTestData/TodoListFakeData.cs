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

        private static Todo CreateTodoNotDone()
        {
            return new(CreateTodoDescription());
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

        public static TodoDescription CreateTodoDescription()
        {
            var generator = new Faker();
            var todoDescription = generator.Random.AlphaNumeric(5);
            return TodoDescription.Create(todoDescription);
        }

        public static TodoList CreateTodoListWithNumberTodosNotDone(
            int numberOfTodosNotDone)
        {
            var todos = Enumerable.Range(0, numberOfTodosNotDone)
                .Select(x => CreateTodoNotDone()).ToList();

            return new TodoList(CreateTodoListName(), CreateTodoListId(), todos);
        }
    }
}