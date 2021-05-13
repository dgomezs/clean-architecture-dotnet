using System.Threading.Tasks;
using Application.Services.Users.Errors;
using Application.Services.Users.UseCases.CreateUser;
using Domain.Shared.Errors;
using Domain.Users.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using WebApi.Auth;
using WebApi.Auth.UserManagement;
using WebApi.Errors;

namespace WebApi.Controllers.Users
{
    [Route("/users")]
    [ApiController]
    public class UserController
    {
        [HttpPost]
        public async Task<string> CreateUser(
            [FromServices] ICreateUserUseCase createUserUseCase,
            [FromServices] IAuthService authService,
            [FromBody] RestCreateUserRequest createUserRequest)
        {
            var (firstName, lastName, email) = createUserRequest;
            var createUserCommand = CreateUserCommand.Create(firstName, lastName,
                email);

            var hasUserSignedUpInAuthSystem =
                await authService.HasUserSignedUpInAuthSystem(createUserCommand.Email);
            if (!hasUserSignedUpInAuthSystem)
                throw new DomainException(new UserHasNotSignedUpError(createUserCommand.Email));

            UserId result = await createUserUseCase.Invoke(
                createUserCommand);
            await authService.AssignUserId(createUserCommand.Email, result);
            return result.Value.ToString();

        }
    }
}