using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services.Repositories;
using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Services.Tests.TestDoubles
{
    public class InMemoryTodoListRepository : ITodoListRepository
    {
        private readonly Dictionary<TodoListId, TodoList> _todoLists = new();

        public Task<TodoList?> GetByName(TodoListName todoListName)
        {
            return Task.FromResult(_todoLists.Values.SingleOrDefault(t => t.Name.Equals(todoListName)));
        }

        public Task Save(TodoList todoList)
        {
            var id = todoList.Id ?? throw new Exception();
            _todoLists.Remove(id);
            _todoLists.Add(id, new TodoList(todoList.Name, todoList.Id, todoList.Todos.ToList()));
            return Task.CompletedTask;
        }

        public async Task RemoveByName(TodoListName todoListName)
        {
            var todoLists = await GetByName(todoListName);
            var id = todoLists?.Id;
            if (id is not null)
            {
                _todoLists.Remove(id);
            }
        }

        public Task<TodoList?> GetById(TodoListId id)
        {
            var valueOrDefault = _todoLists.GetValueOrDefault(id);
            _todoLists.Clear(); // force the entity to be saved again
            return Task.FromResult(valueOrDefault);
        }

        public Task<TodoList?> GetByTodoId(TodoId todoId)
        {
            var valueOrDefault =
                _todoLists.Values.SingleOrDefault(l => l.Todos.Exists(t => todoId.Equals(t.Id)));
            _todoLists.Clear(); // force the entity to be saved again
            return Task.FromResult(valueOrDefault);
        }
    }
}