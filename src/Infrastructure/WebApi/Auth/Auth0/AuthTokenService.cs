using System.Threading.Tasks;
using NodaTime;

namespace WebApi.Auth.Auth0
{
    public class AuthTokenService
    {
        private string _token;
        private Instant _tokenExpiration;
        private readonly IClock _clock;
        private readonly IAuthTokenClient _authTokenClient;

        public AuthTokenService(IAuthTokenClient authTokenClient
            , IClock clock)
        {
            _authTokenClient = authTokenClient;
            _clock = clock;
        }

        public async Task<string> GetToken()
        {
            if (_clock.GetCurrentInstant() < _tokenExpiration)
            {
                return _token;
            }

            var token = await _authTokenClient.GetNewToken();

            _token = token.AccessToken;
            _tokenExpiration = _clock.GetCurrentInstant().Plus(Duration.FromSeconds(token.ExpiresIn));
            return _token;
        }
    }
}