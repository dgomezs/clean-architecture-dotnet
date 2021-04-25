using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Users.ValueObjects;

namespace Application.Services.Todos.UseCases.SearchTodoListByName
{
    public interface ISearchByNameTodoListUseCase
    {
        Task<List<TodoListReadModel>> SearchByName(UserId ownerId, string todoListName);
    }
}