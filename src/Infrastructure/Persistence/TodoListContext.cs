﻿using Domain.Todos.Entities;
using Domain.Users.Entities;
using Infrastructure.Persistence.EfConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public sealed class TodoListContext : DbContext
    {
        public const string Schema = "todos";

        public TodoListContext(DbContextOptions options) : base(options) =>
            ChangeTracker.LazyLoadingEnabled = false;

        public DbSet<TodoList> TodoLists { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);

            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new TodoConfig());
            modelBuilder.ApplyConfiguration(new TodoListConfig());
        }
    }
}