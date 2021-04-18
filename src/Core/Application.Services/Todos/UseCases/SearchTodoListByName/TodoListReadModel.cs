using System;

namespace Application.Services.Todos.UseCases.SearchTodoListByName
{
    public record TodoListReadModel(Guid Id, string Name)
    {
    }
}