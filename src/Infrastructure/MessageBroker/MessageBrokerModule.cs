using Autofac;

namespace MessageBroker
{
    public class MessageBrokerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(new DomainEventPublisher()).AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}