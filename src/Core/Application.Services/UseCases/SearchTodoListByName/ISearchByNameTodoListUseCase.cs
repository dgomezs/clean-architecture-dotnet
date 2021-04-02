using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.UseCases.SearchTodoListByName
{
    public interface ISearchByNameTodoListUseCase
    {
        Task<List<TodoListReadModel>> SearchByName(string todoListName);
    }
}