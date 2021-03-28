﻿using System.Linq;
using System.Threading.Tasks;
using Application.Services.Repositories;
using Domain.Entities;
using Domain.ValueObjects;
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

        public async Task<TodoList?> GetByName(TodoListName todoListName)
        {
            return await _todoListContext.TodoLists.Where(_ => todoListName.Equals(_.Name))
                .SingleOrDefaultAsync();
        }

        public async Task Save(TodoList todoList)
        {
            if (_todoListContext.Entry(todoList).State == EntityState.Detached)
                _todoListContext.TodoLists.Add(todoList);

            await _todoListContext.SaveChangesAsync();
        }
    }
}