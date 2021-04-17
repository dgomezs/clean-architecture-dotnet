using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services.Repositories;
using Domain.Todos.Entities;
using Domain.Users.ValueObjects;

namespace Application.Services.Tests.TestDoubles
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly Dictionary<UserId, Domain.Users.User> _users = new();

        public async Task RemoveByEmail(EmailAddress email)
        {
            var user = await GetByEmail(email);
            var id = user?.Id;
            if (id is not null)
            {
                _users.Remove(id);
            }
        }

        private Task<Domain.Users.User?> GetByEmail(EmailAddress email)
        {
            return Task.FromResult(_users.Values.SingleOrDefault(t => t.Email.Equals(email)));
        }

        public Task<Domain.Users.User?> GetById(UserId userId)
        {
            return Task.FromResult(_users.Values.SingleOrDefault(t => t.Id.Equals(userId)));
        }

        public Task Save(Domain.Users.User user)
        {
            var id = user.Id ?? throw new Exception();
            _users.Remove(id);
            _users.Add(id, new Domain.Users.User(user.Id, user.Email));
            return Task.CompletedTask;
        }
    }
}