using System.Reflection;
using Ardalis.GuardClauses;
using DbUp;
using DbUp.Engine;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence
{
    public class DbUpMigrator
    {
        private const string DbupPasswd = "DBUP_PASSWD";
        private const string DbupUser = "DBUP_USERID";
        private const string DbupConnection = "MSSQL_CONNECTION";

        public DbUpMigrator(IConfiguration config)
        {
            var password =
                Guard.Against.NullOrEmpty(config[DbupPasswd], DbupPasswd);
            var connection =
                Guard.Against.NullOrEmpty(config[DbupConnection],
                    DbupConnection);
            var user = Guard.Against.NullOrEmpty(config[DbupUser], DbupUser);

            ConnectionBuilder = new SqlConnectionStringBuilder(connection)
                {Password = password, UserID = user};
        }

        public SqlConnectionStringBuilder ConnectionBuilder { get; }

        private UpgradeEngine GetUpgradeEngine()
        {
            return DeployChanges.To
                .SqlDatabase(ConnectionBuilder.ConnectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .LogToAutodetectedLog()
                .Build();
        }

        public void DoUpgrade()
        {
            var result = GetUpgradeEngine().PerformUpgrade();
            if (!result.Successful) throw result.Error;
        }
    }
}