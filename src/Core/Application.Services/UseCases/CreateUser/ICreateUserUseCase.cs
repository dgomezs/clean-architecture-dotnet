using System.Threading.Tasks;
using Domain.Users.ValueObjects;

namespace Application.Services.UseCases.CreateUser
{
    public interface ICreateUserUseCase
    {
        Task<UserId> Invoke(CreateUserCommand createUserCommand);
    }
}