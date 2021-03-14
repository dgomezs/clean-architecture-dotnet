using System.Threading.Tasks;
using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Services.Repositories
{
    public interface ITodoListRepository
    {
        Task<TodoList?> GetById(long id);
        Task<TodoList?> GetByName(TodoListName todoListName);
        Task<long> Save(TodoList todoList);
    }
}