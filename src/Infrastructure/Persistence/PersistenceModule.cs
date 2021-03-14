using Autofac;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.NodaTime.Extensions;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence
{
    public class PersistenceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assemblies = typeof(PersistenceModule).Assembly;
            builder.RegisterAutoMapper(assemblies);

            builder.Register(x =>
                {
                    var configuration = x.Resolve<IConfiguration>();
                    return new DbConnectionConfig(configuration);
                })
                .AsSelf().SingleInstance();


            builder.Register(x =>
            {
                var dbConnectionConfig = x.Resolve<DbConnectionConfig>();

                var optionsBuilder =
                    new DbContextOptionsBuilder<TodoListContext>();
                optionsBuilder.UseSqlServer(
                    dbConnectionConfig.ConnectionBuilder.ToString(),
                    x => x.UseNodaTime());
                return new TodoListContext(optionsBuilder.Options);
            }).InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(assemblies)
                .Where(t => t.Name.EndsWith("Query"))
                .InstancePerLifetimeScope()
                .AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(assemblies)
                .Where(t => t.Name.EndsWith("Repository"))
                .InstancePerLifetimeScope()
                .AsImplementedInterfaces();
        }
    }
}