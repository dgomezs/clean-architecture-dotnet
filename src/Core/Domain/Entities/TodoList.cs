using System;
using System.Collections.Generic;
using Ardalis.GuardClauses;
using Domain.Errors;
using Domain.Events;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class TodoList : Aggregate
    {
        private readonly List<Todo> _todos;
        public const int MaxNumberOfTodosNotDoneAllowed = 5;

        public TodoList(TodoListName name, TodoListId id, List<Todo> todos)
        {
            Name = Guard.Against.Null(name, nameof(name));
            Id = Guard.Against.Null(id, nameof(id));
            _todos = Guard.Against.Null(todos, nameof(todos));
        }

        public TodoList(TodoListName name)
        {
            Id = new TodoListId(Guid.NewGuid());
            Name = Guard.Against.Null(name, nameof(name));
            Events.Add(new TodoListCreatedEvent(this));
            _todos = new List<Todo>();
        }

        public TodoListName Name { get; }

        public TodoListId Id { get; }

        public IEnumerable<Todo> Todos => _todos;

        public bool CanIAddTodo()
        {
            return _todos.Count < MaxNumberOfTodosNotDoneAllowed;
        }

        public TodoId AddTodo(TodoDescription todoDescription)
        {
            if (!CanIAddTodo())
            {
                throw new DomainException(new MaxNumberOfTodosUnDoneReachedError(Name, _todos.Count));
            }

            var newTodo = new Todo(todoDescription);
            _todos.Add(newTodo);
            Events.Add(new TodoAddedToListEvent(Id, newTodo));
            return newTodo.Id;
        }
    }
}