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
            _todoLists.Add(id, todoList);
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
            return Task.FromResult(_todoLists.GetValueOrDefault(id));
        }
    }
}