﻿using Application.Services.Todos.UseCases.CreateTodoList;
using Application.Services.Users.UseCases.CreateUser;
using Bogus;
using Domain.Users.ValueObjects;
using FakeTestData;

namespace Application.Services.Tests
{
    public static class FakeCommandGenerator
    {
        public static CreateTodoListCommand FakeCreateTodoListCommand(UserId userId)
        {
            return CreateTodoListCommand.Create(userId, TodoListFakeData
                .CreateTodoListName().Name);
        }

        public static CreateUserCommand FakeCreateUserCommand()
        {
            var personName = UserFakeData.CreatePersonName();
            var email = UserFakeData.CreateEmail();

            return CreateUserCommand.Create(personName.FirstName,
                personName.LastName,
                email.Value);
        }

        public static CreateUserCommand FakeCreateUserCommand(EmailAddress email)
        {
            var generator = new Faker();
            return CreateUserCommand.Create(generator.Person.FirstName,
                generator.Person.LastName,
                email.Value);
        }
    }
}