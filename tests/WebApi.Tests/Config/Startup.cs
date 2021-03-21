using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApi;

namespace CleanArchitecture.TodoList.WebApi.Tests.Config
{
    public class Startup
    {
        public Startup(IConfiguration config, IWebHostEnvironment env) =>
            Configuration = config;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            Install.ConfigureServices(services);
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Install.Configure(app, env);
        }
    }
}