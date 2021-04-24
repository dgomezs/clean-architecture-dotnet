using System.Threading.Tasks;
using Domain.Users.Entities;
using Domain.Users.ValueObjects;

namespace Application.Services.Users.Repositories
{
    public interface IUserRepository
    {
        Task Save(User user);
        Task<User?> GetByEmail(EmailAddress email);
        Task<User?> GetById(UserId userId);
    }
}