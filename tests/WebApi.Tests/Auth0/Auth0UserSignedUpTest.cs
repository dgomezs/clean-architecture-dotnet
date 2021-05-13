using System.Threading.Tasks;
using Auth0.AuthenticationApi;
using Auth0.ManagementApi;
using CleanArchitecture.TodoList.WebApi.Tests.Config;
using Domain.Users.ValueObjects;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NodaTime;
using TestCategories;
using WebApi.Auth.Auth0;
using Xunit;

namespace CleanArchitecture.TodoList.WebApi.Tests.Auth0
{
    public class Auth0UserSignedUpTest
    {
        private readonly Auth0Service _authService;
        private readonly IConfiguration _configuration;

        public Auth0UserSignedUpTest()
        {
            _configuration = ConfigHelper.GetConfig();
            var auth0Config = new Auth0Config(_configuration);
            var authTokenClient = new AuthTokenClient(auth0Config, new HttpClientAuthenticationConnection());
            var authTokenServiceService = new AuthTokenService(authTokenClient, SystemClock.Instance);
            _authService = new Auth0Service(auth0Config, authTokenServiceService,
                new HttpClientManagementConnection());
        }

        [Fact]
        [IntegrationTest]
        public async Task Should_find_user_who_has_signed_up_in_auth_system()
        {
            // arrange
            var existingUserEmail = _configuration["AUTH0_TEST_EMAIL_ADDRESS"];
            // act
            var hasUserSignedUp =
                await _authService.HasUserSignedUpInAuthSystem(EmailAddress.Create(existingUserEmail));
            // assert
            hasUserSignedUp.Should().Be(true);
        }
    }
}