using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Services;
using Autofac;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Extensions.Logging;
using TestEnvironment.Docker;
using TestEnvironment.Docker.Containers.Mssql;
using Xunit;
using Xunit.Abstractions;

namespace Persistence.Tests.Fixtures
{
    public class DbFixture : IAsyncLifetime
    {
        public IContainer Container { get; set; }

        private const int SqlServerPort = 1433;
        public TodoListContext TodoListContext { get; }

        public DbFixture(IMessageSink diagnosticMessageSink)
        {
            Config = ConfigHelper.GetConfig();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Config)
                .WriteTo.TestOutput(diagnosticMessageSink)
                .CreateLogger();

            Container = BuildIoCContainer();

            var dbConfig = Container.Resolve<DbConnectionConfig>();
            TodoListContext = Container.Resolve<TodoListContext>();
            
            DockerEnvironment = BuildDockerEnvironment(dbConfig);
        }

        private IContainer BuildIoCContainer()
        {
            var cfg = new ContainerBuilder();

            cfg.RegisterInstance(new SerilogLoggerFactory(Log.Logger)).AsImplementedInterfaces();

            cfg.RegisterInstance(Config).AsSelf().AsImplementedInterfaces();
            cfg.RegisterModule(new PersistenceModule());
            cfg.RegisterModule(new ApplicationServicesModule());

            return cfg.Build();
        }

        private DockerEnvironment? BuildDockerEnvironment(DbConnectionConfig dbConfig)
        {
            return Config.GetValue<bool>("CREATE_FROM_DOCKER")
                ? new DockerEnvironmentBuilder()
                    .AddMssqlContainer("my-mssql",
                        imageName: "mcr.microsoft.com/mssql/server",
                        saPassword: dbConfig.Password,
                        ports: new Dictionary<ushort, ushort>
                            {{SqlServerPort, SqlServerPort}}, reuseContainer: true)
                    .Build()
                : null;
        }

        private IConfiguration
            Config { get; }

        private DockerEnvironment? DockerEnvironment { get; }

        private IDbContextTransaction? CurrentTransaction { get; set; }


        public async Task InitializeAsync()
        {
            if (DockerEnvironment is not null)
            {
                await DockerEnvironment.Up();
            }

            var dbUpMigrator = new DbUpMigrator(Config);

            dbUpMigrator.DoUpgrade();
            CurrentTransaction =
                await TodoListContext.Database.BeginTransactionAsync();
        }

        public async Task DisposeAsync()
        {
            await CurrentTransaction?.RollbackAsync()!;

            await TodoListContext.DisposeAsync();

            if (DockerEnvironment is not null)
            {
                await DockerEnvironment.Down();
                await DockerEnvironment.DisposeAsync();
            }
        }
    }
}