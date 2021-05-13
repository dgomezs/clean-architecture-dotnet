using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services.Users.Errors;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Domain.Shared.Errors;
using Domain.Users.ValueObjects;
using WebApi.Auth;

namespace Auth0
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

        public async Task AssignUserId(EmailAddress email, UserId userId)
        {
            var users = await GetUsersByEmail(email);
            if (!users.Any())
            {
                throw new DomainException(new UserDoesNotExistError(email));
            }

            var management = await GetManagementApiClient();

            foreach (var user in users)
            {
                await management.Users.UpdateAsync(user.UserId, new UserUpdateRequest
                {
                    UserMetadata = new Auth0UserMetaData(userId.Value.ToString())
                });
            }
        }

        public async Task<bool> HasUserSignedUpInAuthSystem(EmailAddress email)
        {
            var users = await GetUsersByEmail(email);
            return users.Any();
        }

        private async Task<IList<User>> GetUsersByEmail(EmailAddress email)
        {
            var management = await GetManagementApiClient();
            var users = await management.Users.GetUsersByEmailAsync(email.Value);
            return users;
        }

        private async Task<ManagementApiClient> GetManagementApiClient()
        {
            var token = await _authTokenService.GetManagementToken();
            var management =
                new ManagementApiClient(token, new Uri(_config.ManagementApi), _managementConnection);
            return management;
        }
    }
}