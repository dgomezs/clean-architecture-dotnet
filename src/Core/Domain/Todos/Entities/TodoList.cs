using System.Collections.Generic;
using System.Linq;
using Ardalis.GuardClauses;
using Domain.Shared.Entities;
using Domain.Shared.Errors;
using Domain.Todos.Errors;
using Domain.Todos.Events;
using Domain.Todos.TodoValidationRules;
using Domain.Todos.ValueObjects;
using Domain.Users.ValueObjects;
using LanguageExt;

namespace Domain.Todos.Entities
{
    public class TodoList : Aggregate
    {
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


        private Validation<Error, Unit> CanTodoBeAdded(UserId userId)
        {
            var validationRules = new List<IAddTodoValidationRule>
            {
                new OnlyOwnerCanAddTodos(userId),
                new MaxNumberOfTodosReached()
            };

            var errors = validationRules.Select(_ => _.CanTodoBeAdded(this)).Where(_ => _ is not null).ToList();
            return errors.Any()
                ? Validation<Error, Unit>.Fail(errors.ToSeq())
                : Validation<Error, Unit>.Success(Unit.Default);
        }


        public void MarkAsDone(TodoId todoId)
        {
            var todo = _todos.FirstOrDefault(t => todoId.Equals(t.Id)) ??
                       throw new DomainException(new TodoNotFoundError(todoId));
            todo.MarkAsDone();
        }
    }
}