﻿using Domain.Shared.Errors;
using Domain.Todos.Entities;
using Domain.Todos.Errors;
using Domain.Users.ValueObjects;

namespace Domain.Todos.TodoValidationRules
{
    internal class OnlyOwnerCanAddTodos : IAddTodoValidationRule
    {
        public OnlyOwnerCanAddTodos(UserId userThatAddsTodo) =>
            UserThatAddsTodo = userThatAddsTodo;

        private UserId UserThatAddsTodo { get; }

        public Error? CanTodoBeAdded(TodoList todoList)
        {
            return !UserThatAddsTodo.Equals(todoList.OwnerId)
                ? new UserNotAllowedToAddTodoError(UserThatAddsTodo)
                : null;
        }
    }
}