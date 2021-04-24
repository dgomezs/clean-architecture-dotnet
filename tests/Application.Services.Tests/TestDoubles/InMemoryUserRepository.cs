using System.Linq;
using System.Threading.Tasks;
using Application.Services.Users.Repositories;
using Domain.Users.Entities;
using Domain.Users.ValueObjects;

namespace Application.Services.Tests.TestDoubles
{
    public class InMemoryUserRepository : InMemoryRepository<UserId, User>, IUserRepository
    {
        public Task<User?> GetByEmail(EmailAddress email) =>
            Task.FromResult(Elements.Values.SingleOrDefault(t => t.Email.Equals(email)));

        protected override UserId GetId(User user) =>
            user.Id;

        protected override User Copy(User user) =>
            new(new UserId(user.Id.Value), PersonName.Create(user.Name.FirstName, user.Name.LastName),
                EmailAddress.Create(user.Email.Value));

        public async Task RemoveByEmail(EmailAddress email)
        {
            var user = await GetByEmail(email);
            var id = user?.Id;
            if (id is not null)
                Elements.Remove(id);
        }
    }
}