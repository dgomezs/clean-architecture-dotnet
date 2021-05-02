using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Moq;
using WebApi;
using WebApi.Authorization;

namespace CleanArchitecture.TodoList.WebApi.Tests.Config
{
    public class Startup
    {
        public Startup(IConfiguration config, IWebHostEnvironment env) =>
            Configuration = config;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            WebApiInstall.ConfigureServices(services);

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        IssuerSigningKey = FakeJwtManager.SecurityKey,
                        ValidIssuer = FakeJwtManager.Issuer,
                        ValidAudience = FakeJwtManager.Audience
                    };
                });

            var mockAuth = new Mock<IAuthConfig>();
            mockAuth.Setup(_ => _.GetAudience()).Returns(FakeJwtManager.Audience);
            mockAuth.Setup(_ => _.GetIssuer()).Returns(FakeJwtManager.Issuer);

            WebApiInstall.ConfigureAuthorization(services, mockAuth.Object);
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            WebApiInstall.Configure(app, env);
        }
    }
}