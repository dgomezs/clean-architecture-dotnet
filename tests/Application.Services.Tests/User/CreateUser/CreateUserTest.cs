using System.Threading.Tasks;
using Application.Services.Tests.TestDoubles;
using Application.Services.UseCases.CreateUser;
using Autofac.Extras.Moq;
using Domain.Users.ValueObjects;
using Xunit;

namespace Application.Services.Tests.User.CreateUser
{
    public class CreateUserTest
    {
        private readonly ICreateUserUseCase _createUserUseCase;
        private AutoMock _mock;
        private readonly InMemoryUserRepository _userRepository;

        public CreateUserTest()
        {
            _mock = DiConfig.GetMock();
            _userRepository = _mock.Create<InMemoryUserRepository>();
            _createUserUseCase = _mock.Create<ICreateUserUseCase>();
        }


        [Fact]
        public async Task Should_create_new_user_when_it_does_not_exist()
        {
            // arrange
            var createUserCommand = MockDataGenerator.CreateUser();
            await ArrangeUserDoesNotExist(createUserCommand.Email);
            // act
            var userId = await _createUserUseCase.Invoke(createUserCommand);
            // assert
            await AssertUserExists(userId, createUserCommand);
        }


        private async Task AssertUserExists(UserId userId, CreateUserCommand createUserCommand)
        {
            var createdUser = await _userRepository.GetById(userId);
            Assert.Equal(createUserCommand.Email, createdUser.Email);
        }

        private async Task ArrangeUserDoesNotExist(EmailAddress email)
        {
            await _userRepository.RemoveByEmail(email);
        }
    }
}