using System.Linq;
using Domain.Shared.Errors;
using Domain.Todos.Entities;
using Domain.Todos.Errors;

namespace Domain.Todos.TodoValidationRules
{
    public class MaxNumberOfTodosReached : IAddTodoValidationRule
    {
        private const int MaxNumberOfTodosNotDoneAllowed = 5;

        public Error? CanTodoBeAdded(TodoList todoList)
        {
            var todos = todoList.Todos.ToList();
            return todos.Count() >= MaxNumberOfTodosNotDoneAllowed
                ? new MaxNumberOfTodosUnDoneReachedError(todoList.Name, todos.Count)
                : null;
        }
    }
}