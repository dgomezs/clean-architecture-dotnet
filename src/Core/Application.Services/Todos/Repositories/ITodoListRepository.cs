using System.Threading.Tasks;
using Domain.Todos.Entities;
using Domain.Todos.ValueObjects;

namespace Application.Services.Todos.Repositories
{
    public interface ITodoListRepository
    {
        Task<TodoList?> GetById(TodoListId id);
        Task<TodoList?> GetByName(TodoListName todoListName);
        Task<TodoList?> GetByTodoId(TodoId todoId);

        Task Save(TodoList todoList);
    }
}