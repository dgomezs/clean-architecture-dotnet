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
            [FromBody] CreateTodoListRequest createTodoListRequest)
        {
            return await _createTodoListUseCase.Invoke(createTodoListRequest);
        }
    }
}