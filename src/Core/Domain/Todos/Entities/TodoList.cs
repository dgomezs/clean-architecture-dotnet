using System.Collections.Generic;
using System.Linq;
using Application.Services.Todos.Errors;
using Ardalis.GuardClauses;
using Domain.Shared.Entities;
using Domain.Shared.Errors;
using Domain.Todos.Errors;
using Domain.Todos.Events;
using Domain.Todos.ValueObjects;
using Domain.Users.ValueObjects;
using LanguageExt;

namespace Domain.Todos.Entities
{
    public class TodoList : Aggregate
    {
        public const int MaxNumberOfTodosNotDoneAllowed = 5;
        private readonly List<Todo> _todos;

        public TodoList(UserId ownerId, TodoListName name, TodoListId id, List<Todo> todos)
        {
            OwnerId = Guard.Against.Null(ownerId, nameof(ownerId));
            Name = Guard.Against.Null(name, nameof(name));
            Id = Guard.Against.Null(id, nameof(id));
            _todos = Guard.Against.Null(todos, nameof(todos));
        }

        public TodoList(UserId ownerId, TodoListName name)
        {
            Id = new TodoListId();
            OwnerId = Guard.Against.Null(ownerId, nameof(ownerId));
            Name = Guard.Against.Null(name, nameof(name));
            Events.Add(new TodoListCreatedEvent(this));
            _todos = new List<Todo>();
        }

        public TodoListName Name { get; }

        public TodoListId Id { get; }

        public IEnumerable<Todo> Todos => _todos;
        public UserId OwnerId { get; }

        public TodoId AddTodo(UserId userId, TodoDescription todoDescription)
        {
            return CanTodoBeAdded(userId).Match(
                Fail: ex => throw DomainException.FromSeqErrors(ex),
                Succ: _ => AddTodo(todoDescription));
        }

        private TodoId AddTodo(TodoDescription todoDescription)
        {
            var newTodo = new Todo(todoDescription);
            _todos.Add(newTodo);
            Events.Add(new TodoAddedToListEvent(Id, newTodo));
            return newTodo.Id;
        }

        private bool CanUserAddTodo(UserId userId)
        {
            return OwnerId.Equals(userId);
        }

        private Validation<Error, Unit> CanTodoBeAdded(UserId userId)
        {
            if (!CanUserAddTodo(userId))
            {
                return new UserNotAllowedToAddTodoError(userId);
            }

            if (MaxNumberOfTodosReached())
                return new MaxNumberOfTodosUnDoneReachedError(Name, _todos.Count);

            return Unit.Default;
        }


        public void MarkAsDone(TodoId todoId)
        {
            var todo = _todos.FirstOrDefault(t => todoId.Equals(t.Id)) ??
                       throw new DomainException(new TodoNotFoundError(todoId));
            todo.MarkAsDone();
        }

        private bool MaxNumberOfTodosReached()
        {
            return _todos.Count >= MaxNumberOfTodosNotDoneAllowed;
        }
    }
}