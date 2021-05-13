using System.Threading.Tasks;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Bogus;
using FluentAssertions;
using Moq;
using NodaTime;
using NodaTime.Testing;
using TestCategories;
using Xunit;

namespace Auth0.Tests
{
    public class Auth0TokenTest
    {
        private readonly AuthTokenService _authTokenServiceService;
        private readonly FakeClock _clock;
        private readonly Faker _faker;

        public Auth0TokenTest()
        {
            _clock = new FakeClock(SystemClock.Instance.GetCurrentInstant());
            _faker = new Faker();

            var auth0Config = new Auth0Config(ConfigHelper.GetConfig());
            var authTokenClient = new AuthTokenClient(auth0Config, new HttpClientAuthenticationConnection());
            _authTokenServiceService =
                new AuthTokenService(authTokenClient, _clock);
        }

        [Fact]
        [IntegrationTest]
        public async Task Should_get_new_token()
        {
            // act
            var token = await _authTokenServiceService.GetManagementToken();
            // assert
            token.Should().NotBeEmpty();
        }

        [Fact]
        [IntegrationTest]
        public async Task Should_get_same_token_when_token_has_not_expired()
        {
            // act
            var tokenA = await _authTokenServiceService.GetManagementToken();
            var tokenB = await _authTokenServiceService.GetManagementToken();
            // assert
            tokenA.Should().Be(tokenB);
        }

        [Fact]
        public async Task Should_get_new_token_when_token_has_expired()
        {
            // arrange
            const int expirationInSeconds = 10;
            var accessTokenA = CreateToken(expirationInSeconds);
            var accessTokenB = CreateToken(expirationInSeconds);

            var authClient = new Mock<IAuthTokenClient>();
            var authTokenServiceService =
                new AuthTokenService(authClient.Object, _clock);
            authClient.Setup(m => m.GetNewToken())
                .ReturnsAsync(accessTokenA);
            // act
            var tokenA = await authTokenServiceService.GetManagementToken();
            _clock.Advance(Duration.FromSeconds(expirationInSeconds + 1));
            authClient.Setup(m => m.GetNewToken())
                .ReturnsAsync(accessTokenB);
            var tokenB = await authTokenServiceService.GetManagementToken();

            // assert
            tokenA.Should().NotBe(tokenB);
        }

        private AccessTokenResponse CreateToken(int expirationInSeconds)
        {
            return new()
            {
                AccessToken = GenerateRandomString(),
                ExpiresIn = expirationInSeconds
            };
        }

        private string GenerateRandomString()
        {
            return _faker.Random.String();
        }
    }
}