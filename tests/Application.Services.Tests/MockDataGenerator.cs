﻿using System;
using System.Linq;
using Application.Services.Todos.UseCases.CreateTodoList;
using Application.Services.Users.UseCases.CreateUser;
using Bogus;
using Domain.Todos.Entities;
using Domain.Todos.ValueObjects;
using Domain.Users.ValueObjects;

namespace Application.Services.Tests
{
    public static class MockDataGenerator
    {
        public static CreateTodoListCommand CreateTodoListCommand()
        {
            return Todos.UseCases.CreateTodoList.CreateTodoListCommand.Create(CreateTodoListName().Name);
        }

        public static TodoDescription CreateTodoDescription()
        {
            var generator = new Faker();
            var todoDescription = generator.Random.AlphaNumeric(5);
            return TodoDescription.Create(todoDescription);
        }


        public static Domain.Todos.Entities.TodoList CreateTodoListWithNumberTodosNotDone(
            int numberOfTodosNotDone)
        {
            var todos = Enumerable.Range(0, numberOfTodosNotDone)
                .Select(x => CreateTodoNotDone()).ToList();

            return new Domain.Todos.Entities.TodoList(CreateTodoListName(), CreateTodoListId(), todos);
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

        public static CreateUserCommand CreateUserCommand()
        {
            var generator = new Faker();
            return Users.UseCases.CreateUser.CreateUserCommand.Create(generator.Person.FirstName,
                generator.Person.LastName,
                generator.Person.Email);
        }

        public static CreateUserCommand CreateUserCommand(EmailAddress email)
        {
            var generator = new Faker();
            return Users.UseCases.CreateUser.CreateUserCommand.Create(generator.Person.FirstName,
                generator.Person.LastName,
                email.Email);
        }
    }
}