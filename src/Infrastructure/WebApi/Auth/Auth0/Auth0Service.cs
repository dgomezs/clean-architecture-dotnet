using System;
using System.Linq;
using System.Threading.Tasks;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Domain.Users.ValueObjects;

namespace WebApi.Auth.Auth0
{
    public class Auth0Service : IAuthService
    {
        private readonly Auth0Config _config;
        private readonly AuthTokenService _authTokenService;
        private readonly IManagementConnection _managementConnection;

        public Auth0Service(Auth0Config config, AuthTokenService authTokenService
            , IManagementConnection managementConnection)
        {
            _config = config;
            _authTokenService = authTokenService;
            _managementConnection = managementConnection;
        }

        public async Task<bool> HasUserSignedUpInAuthSystem(EmailAddress email)
        {
            var token = await _authTokenService.GetManagementToken();
            var management = new ManagementApiClient(token, new Uri(_config.Domain + "api/v2"), _managementConnection);


            var users = await management.Users.GetUsersByEmailAsync(email.Value);
            return users.Any();
        }
    }
}