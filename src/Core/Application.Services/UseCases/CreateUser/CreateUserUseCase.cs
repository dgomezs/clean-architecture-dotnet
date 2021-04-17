using System.Threading.Tasks;
using Application.Services.Repositories;
using Domain.Users;
using Domain.Users.ValueObjects;

namespace Application.Services.UseCases.CreateUser
{
    public class CreateUserUseCase : ICreateUserUseCase
    {
        private readonly IUserRepository _userRepository;

        public CreateUserUseCase(IUserRepository userRepository) =>
            _userRepository = userRepository;

        public async Task<UserId> Invoke(CreateUserCommand createUserCommand)
        {
            var user = new User(createUserCommand.Email, createUserCommand.Name);
            await _userRepository.Save(user);
            return user.Id;
        }
    }
}