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
        private readonly Dictionary<long, TodoList> _todoLists = new();

        public Task<TodoList?> GetByName(TodoListName todoListName)
        {
            return Task.FromResult(_todoLists.Values.SingleOrDefault(t => t.Name.Equals(todoListName)));
        }

        public Task<TodoListId> Save(TodoList todoList)
        {
            var id = todoList.Id?.Id;
            if (id is not null)
            {
                _todoLists.Remove(id.Value);
            }
            else
            {
                id = _todoLists.Count + 1;
            }

            var todoListId = TodoListId.Create(id.Value);
            _todoLists.Add(id.Value, new TodoList(todoList.Name, todoListId));
            return Task.FromResult(todoListId);
        }

        public async Task RemoveByName(TodoListName todoListName)
        {
            var todoLists = await GetByName(todoListName);
            var id = todoLists?.Id?.Id;
            if (id is not null)
            {
                _todoLists.Remove(id.Value);
            }
        }

        public Task<TodoList?> GetById(TodoListId id)
        {
            return Task.FromResult(_todoLists.GetValueOrDefault(id.Id));
        }
    }
}