using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services.Todos.Queries;
using Application.Services.Todos.UseCases.SearchTodoListByName;
using Domain.Todos.Entities;
using Domain.Users.ValueObjects;
using Infrastructure.Persistence.EfConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Queries
{
    public class SearchTodoListByNameQuery : ISearchTodoListByNameQuery
    {
        private readonly TodoListContext _context;

        public SearchTodoListByNameQuery(TodoListContext context) =>
            _context = context;

        public async Task<List<TodoListReadModel>> SearchByName(UserId userId, string name)
        {
            var sqlQuery =
                $"SELECT * FROM [{TodoListContext.Schema}].[{TodoListConfig.TodolistTable}] \n" +
                $"WHERE  OwnerId = '{userId.Value.ToString()}' AND  Name LIKE '{name}%'";
            return await _context.TodoLists.FromSqlRaw(sqlQuery)
                .AsNoTracking()
                .Select(t => FromTodoList(t)).ToListAsync();
        }

        private static TodoListReadModel FromTodoList(TodoList todoList)
        {
            return new(todoList.Id.Value, todoList.Name.Name);
        }
    }
}