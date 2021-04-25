using Autofac;

namespace Persistence.Tests
{
    internal class PersistenceTestModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assemblies = typeof(PersistenceTestModule).Assembly;
            builder.RegisterAssemblyTypes(assemblies)
                .Where(t => t.Name.EndsWith("ArrangeHelper"))
                .AsSelf()
                .InstancePerLifetimeScope();
        }
    }
}