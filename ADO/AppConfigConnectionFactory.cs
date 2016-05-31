using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Your.Business.ADO
{
    public class AppConfigConnectionFactory : IConnectionFactory
    {
        private readonly DbProviderFactory _provider;
        private readonly string _connectionString;
        private readonly string _name;

        public AppConfigConnectionFactory(string connectionName, ISettings settings)
        {
            if (connectionName == null) throw new ArgumentNullException("connectionName");

            var conStr = settings.ConnectionStrings[connectionName];
            if (conStr == null)
                throw new Exception(string.Format("Failed to find connection string named '{0}' in app/web.config.", connectionName));

            _name = conStr.DatabaseType;
            _provider = DbProviderFactories.GetFactory(conStr.DatabaseType);
            _connectionString = GetConnectionString(connectionName, settings);
        }

        public static string GetConnectionString(string connectionName, ISettings settings)
        {
            if (connectionName == null) throw new ArgumentNullException("connectionName");

            var conStr = settings.ConnectionStrings[connectionName];
            if (conStr == null)
                throw new Exception(string.Format("Failed to find connection string named '{0}' in app/web.config.", connectionName));

            var dbprovider = DbProviderFactories.GetFactory(conStr.DatabaseType);
            var connectionString = string.Empty;
            if (!string.IsNullOrEmpty(conStr.FullString))
                connectionString = conStr.FullString;
            else
            {
                dynamic csb = dbprovider.CreateConnectionStringBuilder();
                csb.DataSource = conStr.DataSource;
                csb.UserID = conStr.UserID;
                csb.Password = conStr.Password;
                csb.PersistSecurityInfo = conStr.PersistSecurityInfo;
                csb.IntegratedSecurity = conStr.IntegratedSecurity;
                connectionString = csb.ConnectionString;
            }
            return connectionString;
        }
        public IDbConnection Create()
        {
            var connection = _provider.CreateConnection();
            if (connection == null)
                throw new Exception(string.Format("Failed to create a connection using the connection string named '{0}' in app/web.config.", _name));

            connection.ConnectionString = _connectionString;
            connection.Open();
            return connection;
        }
    }
}
