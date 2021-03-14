using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EfConfigurations
{
    public class TodoListConfig : IEntityTypeConfiguration<TodoList>
    {
        public void Configure(EntityTypeBuilder<TodoList> todoList)
        {
            todoList.ToTable("TodoList");

            todoList.Property(t => t.Id)
                .ValueGeneratedOnAdd();

            todoList.Property(b => b.Name)
                .IsRequired()
                .HasConversion(
                    v => v.Name,
                    v => TodoListName.Create(v));
            todoList
                .HasKey(t => t.Id);
        }
    }
}