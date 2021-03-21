using Application.Services.UseCases.CreateTodoList;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
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
            ConfigureMocks(services);

            Install.ConfigureServices(services);
        }

        private static void ConfigureMocks(IServiceCollection services)
        {
            Mock<ICreateTodoListUseCase> createTodoListUseCase = new Mock<ICreateTodoListUseCase>();
            services.AddSingleton(x => createTodoListUseCase.Object);
            services.AddSingleton(x => createTodoListUseCase);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Install.Configure(app, env);
        }
    }
}