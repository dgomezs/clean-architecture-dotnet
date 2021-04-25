using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Services.Todos.Repositories;
using Domain.Todos.Entities;
using Domain.Todos.ValueObjects;
using Domain.Users.ValueObjects;
using Infrastructure.Persistence.EfConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class TodoListRepository : ITodoListRepository
    {
        private readonly TodoListContext _todoListContext;

        public TodoListRepository(TodoListContext todoListContext) =>
            _todoListContext = todoListContext;

        public async Task<TodoList?> GetById(TodoListId id)
        {
            return await _todoListContext.TodoLists.Where(_ => id.Equals(_.Id)).SingleOrDefaultAsync();
        }

        public async Task<TodoList?> GetByName(UserId ownerId, TodoListName todoListName)
        {
            return await _todoListContext.TodoLists
                .Where(_ => ownerId.Equals(_.OwnerId) && todoListName.Equals(_.Name))
                .SingleOrDefaultAsync();
        }

        public async Task<TodoList?> GetByTodoId(TodoId todoId)
        {
            var todoListTable = $"[{TodoListContext.Schema}].[{TodoListConfig.TodolistTable}] todoList";
            var todoTable = $"[{TodoListContext.Schema}].[{TodoConfig.TodoTable}] todo";

            var sqlQuery = $"SELECT * FROM  {todoListTable}\n" +
                           $"JOIN ${todoTable} ON todo.TodoListId = todoList.InternalId\n" +
                           $"WHERE todo.Id = {todoId.Value.ToString()}";
            return await _todoListContext.TodoLists.FromSqlRaw(sqlQuery).SingleOrDefaultAsync();
        }

        public async Task Save(TodoList todoList)
        {
            if (_todoListContext.Entry(todoList).State == EntityState.Detached)
            {
                _todoListContext.Entry(todoList).Property<long>(TodoListConfig.InternalOwnerId).CurrentValue =
                    GetInternalIdOwner(todoList);
                _todoListContext.TodoLists.Add(todoList);
            }

            await _todoListContext.SaveChangesAsync();
        }

        private long GetInternalIdOwner(TodoList todoList)
        {
            var owner = _todoListContext.Users.Single(u => todoList.OwnerId.Equals(u.Id)) ??
                        throw new InvalidOperationException();
            return _todoListContext.Entry(owner).Property<long>(UserConfig.IdShadowProperty).CurrentValue;
        }
    }
}