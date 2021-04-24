using Domain.Shared.Errors;
using Domain.Todos.Entities;

namespace Domain.Todos.TodoValidationRules
{
    public interface IAddTodoValidationRule
    {
        public Error? CanTodoBeAdded(TodoList todoList);
    }
}