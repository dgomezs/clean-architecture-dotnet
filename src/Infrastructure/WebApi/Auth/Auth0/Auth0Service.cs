using System.Threading.Tasks;
using Domain.Users.ValueObjects;

namespace WebApi.Auth.Auth0
{
    public class Auth0Service : IAuthService
    {
        private Auth0Config _config;

        public Auth0Service(Auth0Config config) =>
            _config = config;

        public Task<bool> HasUserSignedUpInAuthSystem(EmailAddress create)
        {
            
            
            throw new System.NotImplementedException();
        }
        
    }
}