using Application.Services.Tests.TestDoubles;
using Autofac;
using Autofac.Extras.Moq;

namespace Application.Services.Tests
{
    public class DiConfig
    {
        public static AutoMock GetMock()
        {
            return AutoMock.GetLoose(cfg =>
            {
                cfg.RegisterModule(new ApplicationServicesModule());
                cfg.RegisterInstance(new InMemoryTodoListRepository()).AsSelf().AsImplementedInterfaces();
            });
        }
    }
}