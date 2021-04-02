using System.Threading.Tasks;
using Application.Services.Errors.TodoList;
using Application.Services.Tests.TestDoubles;
using Application.Services.UseCases.AddTodo;
using Application.Services.UseCases.CreateTodoList;
using Autofac.Extras.Moq;
using Domain.Entities;
using Domain.ValueObjects;
using Xunit;

namespace Application.Services.Tests.AddTodoToList
{
    public class AddTodoToListTest
    {
        private AutoMock _mock;
        private readonly InMemoryTodoListRepository _todoListRepository;
        private readonly ICreateTodoListUseCase _createTodoListUseCase;
        private readonly IAddTodoUseCase _addTodoUseCase;

        public AddTodoToListTest()
        {
            _mock = DiConfig.GetMock();
            _todoListRepository = _mock.Create<InMemoryTodoListRepository>();
            _createTodoListUseCase = _mock.Create<ICreateTodoListUseCase>();
            _addTodoUseCase = _mock.Create<IAddTodoUseCase>();
        }

        [Fact]
        public async Task Should_add_a_todo_to_a_list_when_list_exists_and_max_todos_not_reached()
        {
            // arrange
            var createTodoListRequest = MockDataGenerator.CreateTodoList();
            var todoListId = await ArrangeTodoListExist(createTodoListRequest);
            var addTodoCommand = new AddTodoCommand(todoListId, new TodoDescription());
            // act
            var todoId = await _addTodoUseCase.AddTodo(addTodoCommand);
            // assert
            Assert.NotNull(todoId);
        }

        [Fact]
        public async Task Should_throw_an_error_when_todo_list_does_not_exist()
        {
            // arrange
            var createTodoListRequest = MockDataGenerator.CreateTodoList();
            var todoListId = await ArrangeTodoListDoesNotExist(createTodoListRequest);
            var addTodoCommand = new AddTodoCommand(todoListId, new TodoDescription());
            // act / assert
            var exception = await Assert.ThrowsAsync<TodoListDoesNotExistsException>(() =>
                _addTodoUseCase.AddTodo(addTodoCommand));
            var exceptionData = exception.Data;
            Assert.Equal(todoListId.Value.ToString(), exceptionData["TodoListId"]);
        }


        private async Task<TodoListId> ArrangeTodoListExist(CreateTodoListCommand todoListName)
        {
            await _todoListRepository.RemoveByName(todoListName.TodoListName);
            return await _createTodoListUseCase.Invoke(todoListName);
        }

        private async Task<TodoListId> ArrangeTodoListDoesNotExist(CreateTodoListCommand todoListName)
        {
            var id = await _createTodoListUseCase.Invoke(todoListName);
            await _todoListRepository.RemoveByName(todoListName.TodoListName);
            return id;
        }
    }
}