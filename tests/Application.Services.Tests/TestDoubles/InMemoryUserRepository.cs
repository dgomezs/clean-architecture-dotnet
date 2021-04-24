using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services.Users.Repositories;
using Domain.Users.ValueObjects;

namespace Application.Services.Tests.TestDoubles
{
    public class InMemoryUserRepository : InMemoryRepository<UserId, Domain.Users.Entities.User>, IUserRepository
    {
        private readonly Dictionary<UserId, Domain.Users.Entities.User> _users = new();

        public Task<Domain.Users.Entities.User?> GetByEmail(EmailAddress email) =>
            Task.FromResult(_users.Values.SingleOrDefault(t => t.Email.Equals(email)));

        protected override UserId GetId(Domain.Users.Entities.User user) =>
            user.Id;

        protected override Domain.Users.Entities.User Clone(Domain.Users.Entities.User user) =>
            new(user.Id, user.Name, user.Email);

        public async Task RemoveByEmail(EmailAddress email)
        {
            var user = await GetByEmail(email);
            var id = user?.Id;
            if (id is not null)
                _users.Remove(id);
        }
    }
}