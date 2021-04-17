using System.Threading.Tasks;
using Domain.Users;

namespace Application.Services.Repositories
{
    public interface IUserRepository
    {
        Task Save(User user);
    }
}