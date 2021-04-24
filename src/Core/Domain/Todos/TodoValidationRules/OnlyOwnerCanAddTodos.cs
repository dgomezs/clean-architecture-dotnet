using Application.Services.Todos.Errors;
using Domain.Shared.Errors;
using Domain.Todos.Entities;
using Domain.Users.ValueObjects;

namespace Domain.Todos.TodoValidationRules
{
    internal class OnlyOwnerCanAddTodos : IAddTodoValidationRule
    {
        private UserId UserThatAddsTodo { get; }

        public OnlyOwnerCanAddTodos(UserId userThatAddsTodo) =>
            UserThatAddsTodo = userThatAddsTodo;

        public Error? CanTodoBeAdded(TodoList todoList)
        {
            return !UserThatAddsTodo.Equals(todoList.OwnerId)
                ? new UserNotAllowedToAddTodoError(UserThatAddsTodo)
                : null;
        }
    }
}