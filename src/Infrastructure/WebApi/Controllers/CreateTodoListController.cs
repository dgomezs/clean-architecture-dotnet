using System.Threading.Tasks;
using Application.Services.UseCases.CreateTodoList;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("/todo-lists")]
    [ApiController]
    public class CreateTodoListController
    {
        private readonly ICreateTodoListUseCase _createTodoListUseCase;

        public CreateTodoListController(
            ICreateTodoListUseCase createTodoListUseCase) =>
            _createTodoListUseCase = createTodoListUseCase;


        [HttpPost]
        public async Task<long> CreateTodoList(
            [FromBody] RestCreateTodoListRequest createTodoListRequest)
        {
            return await _createTodoListUseCase.Invoke(
                CreateTodoListRequest.Create(createTodoListRequest.Name1, createTodoListRequest.Name2));
        }
    }

    public class RestCreateTodoListRequest
    {
        public string Name1 { get; set; }
        public string Name2 { get; set; }
    }
}