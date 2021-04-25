using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Services.Todos.Queries;
using Ardalis.GuardClauses;
using Domain.Users.ValueObjects;

namespace Application.Services.Todos.UseCases.SearchTodoListByName
{
    public class SearchByNameTodoListUseCase : ISearchByNameTodoListUseCase
    {
        private readonly ISearchTodoListByNameQuery _todoListQueryByName;

        public SearchByNameTodoListUseCase(ISearchTodoListByNameQuery todoListQueryByName) =>
            _todoListQueryByName = todoListQueryByName;

        public Task<List<TodoListReadModel>> SearchByName(UserId userId, string todoListName)
        {
            Guard.Against.NullOrEmpty(todoListName, "name must contain at least one character");
            Guard.Against.Null(userId, "user must be present");
            return _todoListQueryByName.SearchByName(userId, todoListName);
        }
    }
}