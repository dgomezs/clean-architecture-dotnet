using System;
using Auth0.AuthenticationApi;
using Auth0.ManagementApi;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;
using WebApi.Auth;

namespace Auth0
{
    public static class Auth0Install
    {
        public static void ConfigureAuth0(IServiceCollection services, Auth0Config config)
        {
            services.AddSingleton(config);
            services.AddSingleton<IManagementConnection, HttpClientManagementConnection>();
            services.AddSingleton<IAuthenticationConnection, HttpClientAuthenticationConnection>();
            services.AddSingleton<IAuthTokenClient, AuthTokenClient>();
            services.AddSingleton(s =>
                new AuthTokenService(s.GetService<IAuthTokenClient>() ?? throw new InvalidOperationException(),
                    SystemClock.Instance));
            services.AddScoped<IAuthService, Auth0Service>();
        }
    }
}