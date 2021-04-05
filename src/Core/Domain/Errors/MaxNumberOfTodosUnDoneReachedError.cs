using System.Collections;
using System.Collections.Generic;
using Domain.ValueObjects;

namespace Domain.Errors
{
    public record MaxNumberOfTodosUnDoneReachedError : Error
    {
        public const string MaxNumberOfUnDoneTodosReached = "MaxNumberOfUnDoneTodosReached";

        public MaxNumberOfTodosUnDoneReachedError(TodoListName name, int todosCount)
            : base(MaxNumberOfUnDoneTodosReached, "No more todos allowed till you do one")
        {
            TodoListName = name.Name;
            CurrentNumberOfTodos = todosCount;
        }

        public int CurrentNumberOfTodos { get; }
        public string TodoListName { get; }

        public override IDictionary Data => new Dictionary<string, string>()
        {
            {"CurrentNumberOfTodos", CurrentNumberOfTodos + ""},
            {"TodoListName", TodoListName}
        };
    }
}