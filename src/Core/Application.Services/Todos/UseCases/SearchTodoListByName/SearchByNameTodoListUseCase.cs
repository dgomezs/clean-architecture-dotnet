using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Services.Todos.Queries;
using Ardalis.GuardClauses;

namespace Application.Services.Todos.UseCases.SearchTodoListByName
{
    public class SearchByNameTodoListUseCase : ISearchByNameTodoListUseCase
    {
        private readonly ISearchTodoListByNameQuery _todoListQueryByName;

        public SearchByNameTodoListUseCase(ISearchTodoListByNameQuery todoListQueryByName) =>
            _todoListQueryByName = todoListQueryByName;

        public Task<List<TodoListReadModel>> SearchByName(string todoListName)
        {
            Guard.Against.NullOrEmpty(todoListName, "name must contain at list one character");
            return _todoListQueryByName.SearchByName(todoListName);
        }
    }
}