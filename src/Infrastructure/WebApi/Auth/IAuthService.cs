using System.Threading.Tasks;
using Domain.Users.ValueObjects;

namespace WebApi.Auth
{
    public interface IAuthService
    {
        Task<bool> HasUserSignedUpInAuthSystem(EmailAddress email);
        Task AssignUserId(EmailAddress emailAddress, UserId expectedId);
    }
}