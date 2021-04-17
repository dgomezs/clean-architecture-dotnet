using System;
using System.Collections.Generic;
using System.Linq;
using Ardalis.GuardClauses;
using Domain.Shared.Entities;
using Domain.Shared.Errors;
using Domain.Todos.Errors;
using Domain.Todos.Events;
using Domain.Todos.ValueObjects;

namespace Domain.Todos.Entities
{
    public class TodoList : Aggregate
    {
        public const int MaxNumberOfTodosNotDoneAllowed = 5;
        private readonly List<Todo> _todos;

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

        public bool CanAddTodo()
        {
            return _todos.Count < MaxNumberOfTodosNotDoneAllowed;
        }

        public TodoId AddTodo(TodoDescription todoDescription)
        {
            if (!CanAddTodo())
                throw new DomainException(new MaxNumberOfTodosUnDoneReachedError(Name, _todos.Count));

            var newTodo = new Todo(todoDescription);
            _todos.Add(newTodo);
            Events.Add(new TodoAddedToListEvent(Id, newTodo));
            return newTodo.Id;
        }

        public void MarkAsDone(TodoId todoId)
        {
            var todo = _todos.FirstOrDefault(t => todoId.Equals(t.Id)) ??
                       throw new DomainException(new TodoNotFoundError(todoId));
            todo.MarkAsDone();
        }
    }
}