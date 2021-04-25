using Domain.Todos.Entities;
using Domain.Todos.ValueObjects;
using Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EfConfigurations
{
    public class TodoListConfig : IEntityTypeConfiguration<TodoList>
    {
        private const string IdShadowProperty = "InternalId";
        public const string InternalOwnerId = "InternalOwnerId";
        public const string TodolistTable = "TodoList";

        public void Configure(EntityTypeBuilder<TodoList> todoList)
        {
            todoList.ToTable(TodolistTable);

            todoList.Property<long>(IdShadowProperty)
                .HasColumnType("long").ValueGeneratedOnAdd();

            todoList.Property<long>(InternalOwnerId)
                .HasColumnName("InternalOwnerId")
                .IsRequired();

            todoList.Property(t => t.OwnerId)
                .HasConversion(v => v.Value,
                    v => new UserId(v))
                .IsRequired();

            todoList.Property(t => t.Id)
                .IsRequired()
                .HasConversion(v => v.Value,
                    v => new TodoListId(v));

            todoList.Ignore(t => t.DomainEvents);

            todoList.Property(t => t.Name)
                .IsRequired()
                .HasConversion(
                    v => v.Name,
                    v => TodoListName.Create(v));

            todoList.HasMany(b => b.Todos)
                .WithOne()
                .HasForeignKey(TodoConfig.TodoListId)
                .HasPrincipalKey(IdShadowProperty)
                .IsRequired();

            todoList.Metadata
                .FindNavigation("Todos")
                .SetPropertyAccessMode(PropertyAccessMode
                    .FieldDuringConstruction);

            todoList
                .HasKey(IdShadowProperty);
        }
    }
}