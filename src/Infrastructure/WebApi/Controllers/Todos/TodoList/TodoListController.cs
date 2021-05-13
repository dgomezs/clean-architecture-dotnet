using System.Threading.Tasks;
using Application.Services.Todos.UseCases.CreateTodoList;
using Domain.Todos.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Auth.UserManagement;
using static Application.Services.Shared.Extensions.EitherExtensions;

namespace WebApi.Controllers.Todos.TodoList
{
    [Route("/todo-lists")]
    [ApiController]
    public class TodoListController : Controller
    {
        [HttpPost]
        [Authorize("create:todo-lists")]
        public async Task<string> CreateTodoList(
            [FromServices] ICreateTodoListUseCase createTodoListUseCase,
            [FromServices] IUserManager userManager,
            [FromBody] RestCreateTodoListRequest createTodoListRequest)
        {
            var ownerId = await userManager.GetUserId(User);

            // TODO add owner id from authentication
            var createTodoListCommand = CreateTodoListCommand.Create(ownerId, createTodoListRequest.Name);
            TodoListId result = await createTodoListUseCase.InvokeWithErrors(
                    createTodoListCommand)
                .ToThrowException();
            return result.Value.ToString();
        }
    }
}