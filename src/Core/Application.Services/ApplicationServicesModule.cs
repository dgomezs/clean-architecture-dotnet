using Autofac;

namespace Application.Services
{
    public class ApplicationServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assemblies = typeof(ApplicationServicesModule).Assembly;

            builder.RegisterAssemblyTypes(assemblies)
                .Where(t => t.Name.EndsWith("UseCase"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}