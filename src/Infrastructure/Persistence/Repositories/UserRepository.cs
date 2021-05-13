using System.Linq;
using System.Threading.Tasks;
using Application.Services.Users.Repositories;
using Domain.Users.Entities;
using Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TodoListContext _todoListContext;

        public UserRepository(TodoListContext todoListContext) =>
            _todoListContext = todoListContext;

        public async Task Save(User user)
        {
            if (_todoListContext.Entry(user).State == EntityState.Detached)
                _todoListContext.Users.Add(user);

            await _todoListContext.SaveChangesAsync();
        }

        public async Task<User?> GetByEmail(EmailAddress email)
        {
            return await _todoListContext.Users.Where(_ => email.Equals(_.Email)).SingleOrDefaultAsync();
        }

        public async Task<User?> GetById(UserId userId)
        {
            return await _todoListContext.Users.Where(_ => userId.Equals(_.Id)).SingleOrDefaultAsync();
        }
    }
}