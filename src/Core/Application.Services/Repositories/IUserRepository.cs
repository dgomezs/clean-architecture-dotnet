using System.Threading.Tasks;
using Domain.Users;
using Domain.Users.ValueObjects;

namespace Application.Services.Repositories
{
    public interface IUserRepository
    {
        Task Save(User user);
        Task<User?> GetByEmail(EmailAddress email);
    }
}