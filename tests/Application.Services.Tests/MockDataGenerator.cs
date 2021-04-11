﻿using System;
using System.Linq;
using Application.Services.UseCases.CreateTodoList;
using Bogus;
using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Services.Tests
{
    public static class MockDataGenerator
    {
        public static CreateTodoListCommand CreateTodoListCommand()
        {
            return UseCases.CreateTodoList.CreateTodoListCommand.Create(CreateTodoListName().Name);
        }

        public static TodoDescription CreateTodoDescription()
        {
            var generator = new Faker();
            var todoDescription = generator.Random.AlphaNumeric(5);
            return TodoDescription.Create(todoDescription);
        }


        public static TodoList CreateTodoListWithNumberTodosNotDone(int numberOfTodosNotDone)
        {
            var todos = Enumerable.Range(0, numberOfTodosNotDone)
                .Select(x => CreateTodoNotDone()).ToList();

            return new TodoList(CreateTodoListName(), CreateTodoListId(), todos);
        }

        private static TodoListName CreateTodoListName()
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
    }
}