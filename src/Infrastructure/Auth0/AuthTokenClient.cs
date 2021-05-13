using System;
using System.Threading.Tasks;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;

namespace Auth0
{
    public class AuthTokenClient: IAuthTokenClient
    {
        private readonly Auth0Config _config;
        private readonly IAuthenticationConnection _managementConnection;

        public AuthTokenClient(Auth0Config config,
            IAuthenticationConnection managementConnection)
        {
            _config = config;
            _managementConnection = managementConnection;
        }

        public async Task<AccessTokenResponse> GetNewToken()
        {
            var authenticationApiClient =
                new AuthenticationApiClient(new Uri(_config.Domain), _managementConnection);

            var tokenRequest = GetTokenRequest();

            return await authenticationApiClient.GetTokenAsync(tokenRequest);
        }

        private ClientCredentialsTokenRequest GetTokenRequest()
        {
            var tokenRequest = new ClientCredentialsTokenRequest
            {
                Audience = _config.TokenAudience,
                ClientId = _config.ClientId,
                ClientSecret = _config.ClientSecret,
                SigningAlgorithm = JwtSignatureAlgorithm.RS256
            };
            return tokenRequest;
        }
    }

    public interface IAuthTokenClient
    {
        public Task<AccessTokenResponse> GetNewToken();
    }
}