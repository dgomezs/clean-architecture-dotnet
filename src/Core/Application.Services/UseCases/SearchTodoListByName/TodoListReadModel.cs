using System;

namespace Application.Services.UseCases.SearchTodoListByName
{
    public record TodoListReadModel(Guid Id, string Name)
    {
    }
}