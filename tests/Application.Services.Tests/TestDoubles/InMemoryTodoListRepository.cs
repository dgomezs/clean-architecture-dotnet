using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services.Todos.Repositories;
using Domain.Todos.ValueObjects;

namespace Application.Services.Tests.TestDoubles
{
    public class InMemoryTodoListRepository : ITodoListRepository
    {
        private readonly Dictionary<TodoListId, Domain.Todos.Entities.TodoList> _todoLists = new();

        public Task<Domain.Todos.Entities.TodoList?> GetByName(TodoListName todoListName)
        {
            // TODO compare slugify strings
            return Task.FromResult(_todoLists.Values.SingleOrDefault(t => t.Name.Equals(todoListName)));
        }

        public Task Save(Domain.Todos.Entities.TodoList todoList)
        {
            var id = todoList.Id ?? throw new Exception();
            _todoLists.Remove(id);
            _todoLists.Add(id,
                new Domain.Todos.Entities.TodoList(todoList.Name, todoList.Id, todoList.Todos.ToList()));
            return Task.CompletedTask;
        }

        public Task<Domain.Todos.Entities.TodoList?> GetById(TodoListId id)
        {
            var valueOrDefault = _todoLists.GetValueOrDefault(id);
            _todoLists.Clear(); // force the entity to be saved again
            return Task.FromResult(valueOrDefault);
        }

        public Task<Domain.Todos.Entities.TodoList?> GetByTodoId(TodoId todoId)
        {
            var valueOrDefault =
                _todoLists.Values.SingleOrDefault(l => l.Todos.Exists(t => todoId.Equals(t.Id)));
            _todoLists.Clear(); // force the entity to be saved again
            return Task.FromResult(valueOrDefault);
        }

        public async Task RemoveByName(TodoListName todoListName)
        {
            var todoLists = await GetByName(todoListName);
            var id = todoLists?.Id;
            if (id is not null)
                _todoLists.Remove(id);
        }
    }
}