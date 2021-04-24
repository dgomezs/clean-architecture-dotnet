using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services.Todos.Repositories;
using Domain.Todos.Entities;
using Domain.Todos.ValueObjects;
using Domain.Users.ValueObjects;

namespace Application.Services.Tests.TestDoubles
{
    public class InMemoryTodoListRepository : InMemoryRepository<TodoListId, TodoList>,
        ITodoListRepository
    {
        public Task<TodoList?> GetByName(UserId ownerId, TodoListName todoListName)
        {
            // TODO compare slugify strings
            return Task.FromResult(Elements.Values.SingleOrDefault(t => ownerId.Equals(t.OwnerId)
                                                                        && todoListName.Equals(t.Name)));
        }


        protected override TodoListId GetId(TodoList todoList) =>
            todoList.Id;

        protected override TodoList Copy(TodoList todoList) =>
            new(TodoListName.Create(todoList.Name.Name), new TodoListId(todoList.Id.Value),
                Copy(todoList.Todos.ToList()));

        private static List<Todo> Copy(IEnumerable<Todo> todos)
        {
            return todos.Select(Copy).ToList();
        }

        private static Todo Copy(Todo todoList)
        {
            return new(new TodoId(todoList.Id.Value), TodoDescription.Create(todoList.Description.Description),
                todoList.Done);
        }

        public Task<TodoList?> GetByTodoId(TodoId todoId)
        {
            var valueOrDefault =
                Elements.Values.SingleOrDefault(l => l.Todos.Exists(t => todoId.Equals(t.Id)));
            Elements.Clear(); // force the entity to be saved again
            return Task.FromResult(valueOrDefault);
        }

        public async Task RemoveByName(UserId ownerId, TodoListName todoListName)
        {
            var todoLists = await GetByName(ownerId, todoListName);
            var id = todoLists?.Id;
            if (id is not null)
                Elements.Remove(id);
        }

        public async Task RemoveById(TodoListId id)
        {
            Elements.Remove(id);
        }
    }
}