using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using TestEnvironment.Docker;
using TestEnvironment.Docker.Containers.Mssql;
using Xunit;

namespace Persistence.Tests.Fixtures
{
    public class DbFixture : IAsyncLifetime
    {
        public IContainer Container { get; }

        private const int SqlServerPort = 1433;
        public TodoListContext TodoListContext { get; }

        public DbFixture()
        {
            Config = ConfigHelper.GetConfig();
            var cfg = new ContainerBuilder();

            cfg.RegisterInstance(Config).AsSelf().AsImplementedInterfaces();
            cfg.RegisterModule(new PersistenceModule());

            Container = cfg.Build();

            var dbConfig = Container.Resolve<DbConnectionConfig>();
            TodoListContext = Container.Resolve<TodoListContext>();

            DockerEnvironment = new DockerEnvironmentBuilder()
                .AddMssqlContainer("my-mssql",
                    imageName: "mcr.microsoft.com/mssql/server",
                    saPassword: dbConfig.Password,
                    ports: new Dictionary<ushort, ushort>
                        {{SqlServerPort, SqlServerPort}}, reuseContainer: true)
                .Build();
        }

        private IConfiguration
            Config { get; }

        private DockerEnvironment DockerEnvironment { get; }

        private IDbContextTransaction? CurrentTransaction { get; set; }


        public async Task InitializeAsync()
        {
            await DockerEnvironment.Up();

            var dbUpMigrator = new DbUpMigrator(Config);

            dbUpMigrator.DoUpgrade();
            CurrentTransaction =
                await TodoListContext.Database.BeginTransactionAsync();
        }

        public async Task DisposeAsync()
        {
            await CurrentTransaction?.RollbackAsync()!;

            await TodoListContext.DisposeAsync();

            await DockerEnvironment.Down();
            await DockerEnvironment.DisposeAsync();
        }
    }
}