using Ardalis.GuardClauses;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence
{
    public class DbConnectionConfig
    {
        private const string DbPasswd = "MSSQL_PASSWD";
        private const string DbUser = "MSSQL_USERID";
        private const string DbConnectionString = "MSSQL_CONNECTION";

        public DbConnectionConfig(IConfiguration config)
        {
            var password =
                Guard.Against.NullOrEmpty(config[DbPasswd], DbPasswd);
            var connection =
                Guard.Against.NullOrEmpty(config[DbConnectionString],
                    DbConnectionString);
            var user = Guard.Against.NullOrEmpty(config[DbUser], DbUser);

            ConnectionBuilder = new SqlConnectionStringBuilder(connection)
                {Password = password, UserID = user};
        }


        public SqlConnectionStringBuilder ConnectionBuilder { get; }

        public string Password => ConnectionBuilder.Password;
    }
}