using Domain.Todos.Entities;
using Domain.Users;
using Infrastructure.Persistence.EfConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public sealed class TodoListContext : DbContext
    {
        public TodoListContext(DbContextOptions options) : base(options) =>
            ChangeTracker.LazyLoadingEnabled = false;

        public DbSet<TodoList> TodoLists { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("todos");

            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new TodoConfig());
            modelBuilder.ApplyConfiguration(new TodoListConfig());
        }
    }
}