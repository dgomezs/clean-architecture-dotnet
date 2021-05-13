using System.Threading.Tasks;
using NodaTime;

namespace Auth0
{
    public class AuthTokenService
    {
        private readonly IAuthTokenClient _authTokenClient;
        private readonly IClock _clock;
        private string _token;
        private Instant _tokenExpiration;

        public AuthTokenService(IAuthTokenClient authTokenClient
            , IClock clock)
        {
            _authTokenClient = authTokenClient;
            _clock = clock;
        }

        public async Task<string> GetManagementToken()
        {
            if (_clock.GetCurrentInstant() < _tokenExpiration)
                return _token;

            var token = await _authTokenClient.GetNewToken();

            _token = token.AccessToken;
            _tokenExpiration = _clock.GetCurrentInstant().Plus(Duration.FromSeconds(token.ExpiresIn));
            return _token;
        }
    }
}