using System;
using System.Threading.Tasks;
using Application.Services.Users.Repositories;
using Autofac;
using FakeTestData;
using Persistence.Tests.Fixtures;
using Xunit;

namespace Persistence.Tests.Users.PersistUser
{
    [Collection("DB")]
    public class PersistUser
    {
        private readonly IUserRepository _userRepository;

        public PersistUser(DbFixture dbFixture) =>
            _userRepository = dbFixture.Container.Resolve<IUserRepository>();

        [Fact]
        public async Task Should_persist_a_new_user()
        {
            // arrange
            var user = UserFakeData.CreateUser();
            // act
            await _userRepository.Save(user);
            // assert
            var persistedUser = await _userRepository.GetByEmail(user.Email) ?? throw new Exception();
            Assert.Equal(user.Email, persistedUser.Email);
            Assert.Equal(user.Name, persistedUser.Name);
        }
    }
}