using System;
using Auth0.AuthenticationApi;
using Auth0.ManagementApi;
using Autofac;
using Microsoft.Extensions.Configuration;
using NodaTime;

namespace Auth0
{
    public class Auth0Module : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(x =>
                {
                    var configuration = x.Resolve<IConfiguration>();
                    return new Auth0Config(configuration);
                })
                .AsSelf().SingleInstance();


            builder.RegisterType<HttpClientManagementConnection>().AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<HttpClientAuthenticationConnection>().AsImplementedInterfaces()
                .SingleInstance();
            builder.RegisterType<AuthTokenClient>().AsImplementedInterfaces()
                .SingleInstance();


            builder.Register(s => new AuthTokenService(
                s.Resolve<IAuthTokenClient>() ?? throw new InvalidOperationException(),
                SystemClock.Instance)).SingleInstance();
            builder.RegisterType<Auth0Service>().AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}