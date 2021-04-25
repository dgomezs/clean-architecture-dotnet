using System.Threading.Tasks;
using Application.Services.Users.UseCases.CreateUser;
using Domain.Users.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Users
{
    [Route("/users")]
    [ApiController]
    public class UserController
    {
        [HttpPost]
        public async Task<string> CreateUser(
            [FromServices] ICreateUserUseCase createUserUseCase,
            [FromBody] RestCreateUserRequest createUserRequest)
        {
            var (firstName, lastName, email) = createUserRequest;
            var createUserCommand = CreateUserCommand.Create(firstName, lastName,
                email);
            UserId result = await createUserUseCase.Invoke(
                createUserCommand);
            return result.Value.ToString();
        }
    }
}