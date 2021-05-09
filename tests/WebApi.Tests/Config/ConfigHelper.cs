using Microsoft.Extensions.Configuration;

namespace CleanArchitecture.TodoList.WebApi.Tests.Config
{
    public static class ConfigHelper
    {
        public static IConfiguration GetConfig()
        {
            return new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddUserSecrets<Startup>()
                .Build();
        }
    }
}