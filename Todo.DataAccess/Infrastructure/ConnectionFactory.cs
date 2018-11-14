using Dapper.FastCrud;
using System.Data;
using System.Data.SqlClient;

namespace Todo.DataAccess.Infrastructure
{
    public class ConnectionFactory : IConnectionFactory
    {

        private readonly string _connectionString;
        public SqlConnection Connection;
        public ConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
            
        }

        public IDbConnection GetConnection
        {
            get
            {
                OrmConfiguration.DefaultDialect = SqlDialect.MsSql;
                return new SqlConnection(_connectionString);
            }
        }


        #region IDisposable Support
        private bool disposedValue = false; 

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
