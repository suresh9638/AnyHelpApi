
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;

namespace anyhelp.Data.DataContext
{


    public interface IDatabaseConnectionFactory
    {
        IDbConnection CreateConnection();
    }

    public class SqlConnectionFactory : IDatabaseConnectionFactory, IDisposable
    {
        public string _connectionString;
        AppConfiguration appConfiguration = new AppConfiguration();

        public SqlConnectionFactory()
        {
            _connectionString = appConfiguration.SqlConnectonString;
        }
        public IDbConnection CreateConnection()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(_connectionString);
            builder.ConnectTimeout = 10;
            var sqlConnection = new SqlConnection(builder.ToString());
            if (sqlConnection.State == ConnectionState.Closed)
            {                
                sqlConnection.Open();
            }
            return sqlConnection;
        }
        private bool disposed = false;
        public void Dispose()
        {
            Dispose(disposing: true);
           
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            SqlConnection.ClearAllPools();
            
        }
    }

}
