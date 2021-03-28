using Application.Services;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutofacSerilogIntegration;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApi;

namespace App
{
    public class Startup
    {
        public Startup(IConfiguration config, IWebHostEnvironment env) =>
            Configuration = config;

        public ILifetimeScope? AutofacContainer { get; private set; }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            WebApiInstall.ConfigureServices(services);
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterLogger();
            builder.Register(_ => Configuration).AsSelf().SingleInstance();
            builder.RegisterModule(new PersistenceModule());
            builder.RegisterModule(new ApplicationServicesModule());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            WebApiInstall.Configure(app, env);
        }
    }
}