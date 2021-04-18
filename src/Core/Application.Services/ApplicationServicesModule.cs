using Application.Services.Shared.Events;
using Autofac;

namespace Application.Services
{
    public class ApplicationServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assemblies = typeof(ApplicationServicesModule).Assembly;

            builder.Register(x => new DomainEventPublisher()).AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(assemblies)
                .Where(t => t.Name.EndsWith("UseCase"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}