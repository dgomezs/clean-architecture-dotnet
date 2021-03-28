using Microsoft.Extensions.Configuration;
using Persistence.Tests.Fixtures;

namespace Persistence.Tests
{
    public static class ConfigHelper
    {
        public static IConfiguration GetConfig()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .AddEnvironmentVariables()
                .AddUserSecrets<DbFixture>()
                .Build();
        }
    }
}