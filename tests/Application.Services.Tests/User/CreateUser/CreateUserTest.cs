using System;
using System.Threading.Tasks;
using Application.Services.Tests.TestDoubles;
using Application.Services.Users.UseCases.CreateUser;
using Autofac.Extras.Moq;
using Domain.Shared.Errors;
using Domain.Users.ValueObjects;
using Xunit;

namespace Application.Services.Tests.User.CreateUser
{
    public class CreateUserTest
    {
        private readonly ICreateUserUseCase _createUserUseCase;
        private readonly AutoMock _mock;
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
            var createUserCommand = MockDataGenerator.CreateUserCommand();
            await ArrangeUserDoesNotExist(createUserCommand.Email);
            // act
            var userId = await _createUserUseCase.Invoke(createUserCommand);
            // assert
            await AssertUserExists(userId, createUserCommand);
        }

        [Fact]
        public async Task Should_throw_error_when_user_with_same_email_exists()
        {
            // arrange
            var existingUser = await CreateUser();
            var createUserCommand = MockDataGenerator.CreateUserCommand(existingUser.Email);
            // act
            var exception = await Assert.ThrowsAsync<DomainException>(() =>
                _createUserUseCase.Invoke(createUserCommand));
            // assert
            var exceptionData = exception.Data;
            Assert.Equal(existingUser.Email.Email, exceptionData["Email"]);
        }

        // TODO add cases for invalid names and email

        private async Task<Domain.Users.User> CreateUser()
        {
            var createUserCommand = MockDataGenerator.CreateUserCommand();
            await ArrangeUserDoesNotExist(createUserCommand.Email);
            // act
            var userId = await _createUserUseCase.Invoke(createUserCommand);
            return await _userRepository.GetById(userId) ?? throw new Exception();
        }


        private async Task AssertUserExists(UserId userId, CreateUserCommand createUserCommand)
        {
            var createdUser = await _userRepository.GetById(userId) ?? throw new Exception();
            Assert.Equal(createUserCommand.Email, createdUser.Email);
            Assert.Equal(createUserCommand.Name, createdUser.Name);
        }

        private async Task ArrangeUserDoesNotExist(EmailAddress email)
        {
            await _userRepository.RemoveByEmail(email);
        }
    }
}