using System;
using System.Threading.Tasks;
using Application.Services.Users.Repositories;
using Domain.Users.Entities;
using FakeTestData;

namespace Persistence.Tests.Helpers
{
    public class UserArrangeHelper
    {
        private readonly IUserRepository _userRepository;

        public UserArrangeHelper(IUserRepository userRepository) =>
            _userRepository = userRepository;

        public async Task<User> CreateUser()
        {
            var user = UserFakeData.CreateUser();
            await _userRepository.Save(user);
            return await _userRepository.GetById(user.Id) ?? throw new Exception();
        }
    }
}