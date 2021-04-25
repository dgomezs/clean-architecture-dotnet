using Domain.Todos.Entities;
using Domain.Todos.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EfConfigurations
{
    public class TodoConfig : IEntityTypeConfiguration<Todo>
    {
        private const string IdShadowProperty = "InternalId";
        public const string TodoListId = "TodoListId";
        public const string TodoTable = "Todo";

        public void Configure(EntityTypeBuilder<Todo> todo)
        {
            todo.ToTable(TodoTable);

            todo.Property<long>(IdShadowProperty)
                .HasColumnType("long").ValueGeneratedOnAdd();

            todo.Property<long>(TodoListId).IsRequired();

            todo.Property(t => t.Id)
                .IsRequired()
                .HasConversion(v => v.Value,
                    v => new TodoId(v));

            todo.Ignore(t => t.DomainEvents);

            todo.Property(t => t.Description)
                .IsRequired()
                .HasConversion(
                    v => v.Description,
                    v => TodoDescription.Create(v));
            todo
                .HasKey(IdShadowProperty);
        }
    }
}