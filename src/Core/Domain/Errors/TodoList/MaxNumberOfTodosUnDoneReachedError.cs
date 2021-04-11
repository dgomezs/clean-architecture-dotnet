using System;
using System.Collections;
using System.Collections.Generic;
using Domain.ValueObjects;

namespace Domain.Errors.TodoList
{
    public record MaxNumberOfTodosUnDoneReachedError : Error
    {
        private const string MaxNumberOfUnDoneTodosReached = "MaxNumberOfUnDoneTodosReached";

        public MaxNumberOfTodosUnDoneReachedError(TodoListName name, int todosCount)
            : base(MaxNumberOfUnDoneTodosReached, "No more todos allowed till you do one")
        {
            TodoListName = name.Name;
            CurrentNumberOfTodos = todosCount;
        }

        private int CurrentNumberOfTodos { get; }
        private string TodoListName { get; }

        public override IDictionary Data => new Dictionary<string, string>()
        {
            {"CurrentNumberOfTodos", Convert.ToString(CurrentNumberOfTodos)},
            {"TodoListName", TodoListName}
        };
    }
}