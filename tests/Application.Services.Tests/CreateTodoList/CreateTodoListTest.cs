using System.Threading.Tasks;
using Application.Services.Errors;
using Application.Services.Tests.TestDoubles;
using Application.Services.UseCases.CreateTodoList;
using Autofac.Extras.Moq;
using Domain.ValueObjects;
using Xunit;

namespace Application.Services.Tests.CreateTodoList
{
    public class CreateTodoListTest
    {
        private readonly ICreateTodoListUseCase _createTodoListUseCase;
        private readonly InMemoryTodoListRepository _todoListRepository;
        private readonly AutoMock _mock;

        public CreateTodoListTest()
        {
            _mock = DiConfig.GetMock();
            _createTodoListUseCase = _mock.Create<ICreateTodoListUseCase>();
            _todoListRepository = _mock.Create<InMemoryTodoListRepository>();
        }

        [Fact]
        public async Task Should_create_new_todolist_when_does_not_exist()
        {
            // arrange
            var createTodoListRequest = MockDataGenerator.CreateTodoList();
            var todoListName = createTodoListRequest.TodoListName;
            await TodoListDoesNotExist(todoListName);
            // act
            var id = await _createTodoListUseCase.Invoke(createTodoListRequest);
            // assert
            var todoList = await _todoListRepository.GetById(id);
            Assert.Equal(todoListName, todoList?.Name);
            Assert.Equal(id, todoList?.Id);
        }

        [Fact]
        public async Task Should_not_create_new_todolist_when_one_by_same_name_exists()
        {
            // arrange
            var createTodoListRequest = MockDataGenerator.CreateTodoList();
            await TodoListDoesNotExist(createTodoListRequest.TodoListName);
            await _createTodoListUseCase.Invoke(createTodoListRequest);

            // act
            // create another list by the same n
            await Assert.ThrowsAsync<TodoListAlreadyExistsException>(() =>
                _createTodoListUseCase.Invoke(createTodoListRequest));
        }


        private async Task TodoListDoesNotExist(TodoListName todoListName)
        {
            await _todoListRepository.RemoveByName(todoListName);
        }
    }
}