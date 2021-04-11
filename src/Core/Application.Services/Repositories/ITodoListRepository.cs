using System.Threading.Tasks;
using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Services.Repositories
{
    public interface ITodoListRepository
    {
        Task<TodoList?> GetById(TodoListId id);
        Task<TodoList?> GetByName(TodoListName todoListName);
        Task<TodoList?> GetByTodoId(TodoId todoId);

        Task Save(TodoList todoList);
    }
}