using System.Threading.Tasks;
using Application.Services.Users.UseCases.CreateUser;
using Domain.Users.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Users
{
    [Route("/users")]
    [ApiController]
    public class CreateUserController
    {
        private readonly ICreateUserUseCase _createUserUseCase;

        public CreateUserController(
            ICreateUserUseCase createUserUseCase) =>
            _createUserUseCase = createUserUseCase;


        [HttpPost]
        public async Task<string> CreateUser(
            [FromBody] RestCreateUserRequest createUserRequest)
        {
            var (firstName, lastName, email) = createUserRequest;
            UserId result = await _createUserUseCase.Invoke(
                CreateUserCommand.Create(firstName, lastName,
                    email));
            return result.Value.ToString();
        }
    }
}