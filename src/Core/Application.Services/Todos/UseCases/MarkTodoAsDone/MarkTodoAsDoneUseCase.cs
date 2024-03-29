﻿using System.Threading.Tasks;
using Application.Services.Todos.Errors;
using Application.Services.Todos.Repositories;
using Domain.Shared.Errors;

namespace Application.Services.Todos.UseCases.MarkTodoAsDone
{
    public class MarkTodoAsDoneUseCase : IMarkTodoDoneUseCase
    {
        private readonly ITodoListRepository _todoListRepository;

        public MarkTodoAsDoneUseCase(ITodoListRepository todoListRepository) =>
            _todoListRepository = todoListRepository;

        public async Task Invoke(MarkTodoDoneCommand markTodoDoneCommand)
        {
            var todoId = markTodoDoneCommand.TodoId;
            var todoList = await _todoListRepository.GetByTodoId(todoId)
                           ?? throw new DomainException(
                               new TodoListDoesNotExistsError(todoId));
            todoList.MarkAsDone(todoId);
            await _todoListRepository.Save(todoList);
        }
    }
}