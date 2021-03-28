using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EfConfigurations
{
    public class TodoListConfig : IEntityTypeConfiguration<TodoList>
    {
        public const string IdShadowProperty = "InternalId";

        public void Configure(EntityTypeBuilder<TodoList> todoList)
        {
            todoList.ToTable("TodoList");

            todoList.Property<long>(IdShadowProperty)
                .HasColumnType("long").ValueGeneratedOnAdd();

            todoList.Property(t => t.Id)
                .IsRequired()
                .HasConversion(v => v.Value,
                    v => new TodoListId(v));

            todoList.Property(b => b.Name)
                .IsRequired()
                .HasConversion(
                    v => v.Name,
                    v => TodoListName.Create(v));
            todoList
                .HasKey(IdShadowProperty);
        }
    }
}