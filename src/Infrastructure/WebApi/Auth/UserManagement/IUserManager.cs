using System.Security.Claims;
using System.Threading.Tasks;
using Domain.Users.ValueObjects;

namespace WebApi.Auth.UserManagement
{
    public interface IUserManager
    {
        Task<UserId> GetUserId(ClaimsPrincipal claimsPrincipal);
        Task<bool> HasUserSignedUpInAuthSystem(EmailAddress create);
    }
}