using System.Threading.Tasks;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using CleanArchitecture.TodoList.WebApi.Tests.Config;
using FluentAssertions;
using Moq;
using NodaTime;
using NodaTime.Testing;
using WebApi.Auth.Auth0;
using Xunit;

namespace CleanArchitecture.TodoList.WebApi.Tests.Auth0
{
    public class Auth0TokenTest
    {
        private readonly AuthTokenService _authTokenServiceService;
        private readonly FakeClock _clock;

        public Auth0TokenTest()
        {
            _clock = new FakeClock(SystemClock.Instance.GetCurrentInstant());

            var auth0Config = new Auth0Config(ConfigHelper.GetConfig());
            var authTokenClient = new AuthTokenClient(auth0Config, new HttpClientAuthenticationConnection());
            _authTokenServiceService =
                new AuthTokenService(authTokenClient, _clock);
        }

        [Fact]
        public async Task Should_get_new_token()
        {
            // act
            var token = await _authTokenServiceService.GetToken();
            // assert
            token.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Should_get_same_token_when_token_has_not_expired()
        {
            // act
            var tokenA = await _authTokenServiceService.GetToken();
            var tokenB = await _authTokenServiceService.GetToken();
            // assert
            tokenA.Should().Be(tokenB);
        }

        [Fact]
        public async Task Should_get_new_token_when_token_has_expired()
        {
            // arrange
            var expirationInSeconds = 10;
            var authClient = new Mock<IAuthTokenClient>();
            var accessTokenA = new AccessTokenResponse
            {
                AccessToken = "TokenA",
                ExpiresIn = expirationInSeconds
            };
            var accessTokenB = new AccessTokenResponse
            {
                AccessToken = "TokenB",
                ExpiresIn = expirationInSeconds
            };

            var authTokenServiceService =
                new AuthTokenService(authClient.Object, _clock);
            authClient.Setup(m => m.GetNewToken())
                .ReturnsAsync(accessTokenA);
            // act
            var tokenA = await authTokenServiceService.GetToken();
            _clock.Advance(Duration.FromSeconds(expirationInSeconds + 1));
            authClient.Setup(m => m.GetNewToken())
                .ReturnsAsync(accessTokenB);
            var tokenB = await authTokenServiceService.GetToken();

            // assert
            tokenA.Should().NotBe(tokenB);
        }
    }
}