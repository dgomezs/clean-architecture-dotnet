using Application.Services.Users.Repositories;
using Autofac;
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
    }
}