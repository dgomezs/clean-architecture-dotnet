using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Services.UseCases.SearchTodoListByName;

namespace Application.Services.Queries
{
    public interface ISearchTodoListByNameQuery
    {
        Task<List<TodoListReadModel>> SearchByName(string name);
    }
}