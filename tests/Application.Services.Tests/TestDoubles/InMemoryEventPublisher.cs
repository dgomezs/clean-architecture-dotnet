﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Services.Shared.Events;
using Domain.Shared.Events;

namespace Application.Services.Tests.TestDoubles
{
    public class InMemoryEventPublisher : IDomainEventPublisher
    {
        public readonly List<DomainEvent> Events = new();

        public Task PublishEvent(DomainEvent evt)
        {
            Events.Add(evt);
            return Task.CompletedTask;
        }

        public Task PublishEvents(IEnumerable<DomainEvent> events)
        {
            Events.AddRange(events);
            return Task.CompletedTask;
        }

        public void ClearEvents()
        {
            Events.Clear();
        }
    }
}