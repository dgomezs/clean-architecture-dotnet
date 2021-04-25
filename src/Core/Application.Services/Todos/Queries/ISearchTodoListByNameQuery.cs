using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Services.Todos.UseCases.SearchTodoListByName;
using Domain.Users.ValueObjects;

namespace Application.Services.Todos.Queries
{
    public interface ISearchTodoListByNameQuery
    {
        Task<List<TodoListReadModel>> SearchByName(UserId ownerId, string name);
    }
}