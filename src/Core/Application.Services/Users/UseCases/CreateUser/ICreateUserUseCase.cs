using System.Threading.Tasks;
using Domain.Users.ValueObjects;

namespace Application.Services.Users.UseCases.CreateUser
{
    public interface ICreateUserUseCase
    {
        Task<UserId> Invoke(CreateUserCommand createUserCommand);
    }
}