using System;
using System.Collections.Generic;
using Domain.Events;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class TodoList
    {
        private readonly List<DomainEvent> _domainEvents;

        public TodoList(TodoListName name, TodoListId id)
        {
            Name = name;
            Id = id;
            _domainEvents = new List<DomainEvent>();
        }

        public TodoList(TodoListName name)
        {
            Id = new TodoListId(Guid.NewGuid());
            Name = name;
            _domainEvents = new List<DomainEvent>();
            _domainEvents.Add(new TodoListCreatedEvent(this));
        }

        public TodoListName Name { get; }

        public TodoListId Id { get; }

        public IEnumerable<DomainEvent> DomainEvents => _domainEvents;
    }
}