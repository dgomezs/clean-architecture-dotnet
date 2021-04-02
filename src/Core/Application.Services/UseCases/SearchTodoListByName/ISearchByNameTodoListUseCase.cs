using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.ValueObjects;

namespace Application.Services.UseCases.SearchTodoListByName
{
    public interface ISearchByNameTodoListUseCase
    {
        Task<List<TodoListReadModel>> SearchByName(string todoListName);
    }
}