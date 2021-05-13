using Microsoft.Extensions.Configuration;

namespace Auth0.Tests
{
    public static class ConfigHelper
    {
        public static IConfiguration GetConfig()
        {
            return new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddUserSecrets<Auth0TokenTest>()
                .Build();
        }
    }
}