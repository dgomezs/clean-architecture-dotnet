using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Services.Todos.UseCases.SearchTodoListByName;

namespace Application.Services.Todos.Queries
{
    public interface ISearchTodoListByNameQuery
    {
        Task<List<TodoListReadModel>> SearchByName(string name);
    }
}