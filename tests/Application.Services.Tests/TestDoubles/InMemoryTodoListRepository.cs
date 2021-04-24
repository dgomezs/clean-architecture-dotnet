using System.Linq;
using System.Threading.Tasks;
using Application.Services.Todos.Repositories;
using Domain.Todos.ValueObjects;

namespace Application.Services.Tests.TestDoubles
{
    public class InMemoryTodoListRepository : InMemoryRepository<TodoListId, Domain.Todos.Entities.TodoList>,
        ITodoListRepository
    {
        public Task<Domain.Todos.Entities.TodoList?> GetByName(TodoListName todoListName)
        {
            // TODO compare slugify strings
            return Task.FromResult(Elements.Values.SingleOrDefault(t => t.Name.Equals(todoListName)));
        }


        protected override TodoListId GetId(Domain.Todos.Entities.TodoList todoList) =>
            todoList.Id;

        protected override Domain.Todos.Entities.TodoList Clone(Domain.Todos.Entities.TodoList todoList) =>
            new(todoList.Name, todoList.Id, todoList.Todos.ToList());

        public Task<Domain.Todos.Entities.TodoList?> GetByTodoId(TodoId todoId)
        {
            var valueOrDefault =
                Elements.Values.SingleOrDefault(l => l.Todos.Exists(t => todoId.Equals(t.Id)));
            Elements.Clear(); // force the entity to be saved again
            return Task.FromResult(valueOrDefault);
        }

        public async Task RemoveByName(TodoListName todoListName)
        {
            var todoLists = await GetByName(todoListName);
            var id = todoLists?.Id;
            if (id is not null)
                Elements.Remove(id);
        }
    }
}