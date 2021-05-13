using System;
using System.Linq;
using System.Threading.Tasks;
using Auth0.AuthenticationApi;
using Auth0.ManagementApi;
using Domain.Users.ValueObjects;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using NodaTime;
using TestCategories;
using Xunit;

namespace Auth0.Tests
{
    public class Auth0UserSignedUpTest
    {
        private readonly Auth0Service _authService;
        private readonly IConfiguration _configuration;
        private AuthTokenService _authTokenServiceService;
        private EmailAddress _existingUserEmail;
        private HttpClientAuthenticationConnection _httpClientAuthenticationConnection;
        private HttpClientManagementConnection _httpClientManagementConnection;
        private Auth0Config _auth0Config;

        public Auth0UserSignedUpTest()
        {
            _configuration = ConfigHelper.GetConfig();
            _auth0Config = new Auth0Config(_configuration);
            _httpClientAuthenticationConnection = new HttpClientAuthenticationConnection();
            var authTokenClient = new AuthTokenClient(_auth0Config, _httpClientAuthenticationConnection);
            _authTokenServiceService = new AuthTokenService(authTokenClient, SystemClock.Instance);
            _httpClientManagementConnection = new HttpClientManagementConnection();
            _authService = new Auth0Service(_auth0Config, _authTokenServiceService,
                _httpClientManagementConnection);
            _existingUserEmail = EmailAddress.Create(_configuration["AUTH0_TEST_EMAIL_ADDRESS"]);
        }

        [Fact]
        [IntegrationTest]
        public async Task Should_find_user_who_has_signed_up_in_auth_system()
        {
            // act
            var hasUserSignedUp =
                await _authService.HasUserSignedUpInAuthSystem(_existingUserEmail);
            // assert
            hasUserSignedUp.Should().Be(true);
        }

        [Fact]
        [IntegrationTest]
        public async Task Should_assign_id_to_user()
        {
            // arrange
            var userId = new UserId();
            // act
            await _authService.AssignUserId(_existingUserEmail, userId);
            // assert
            var managementApiClient = await GetManagementClient();
            var users = await managementApiClient.Users.GetUsersByEmailAsync(_existingUserEmail.Value);
            Assert.Single(users);
            JObject userMetadata = users.First().UserMetadata;
            Assert.Equal(userId.Value.ToString(),
                (userMetadata.GetValue(nameof(Auth0UserMetaData.TodoListUserId)) ??
                 throw new InvalidOperationException()).Value<string>());
        }


        private async Task<ManagementApiClient> GetManagementClient()
        {
            var token = await _authTokenServiceService.GetManagementToken();
            return new ManagementApiClient(token, new Uri(_auth0Config.ManagementApi),
                _httpClientManagementConnection);
        }
    }
}