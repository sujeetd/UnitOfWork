using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Your.Business.ADO
{
    public class AdoNetContext : IContext
    {
        private readonly IDbConnection _connection;
        private readonly IConnectionFactory _connectionFactory;
        public IDbConnection Connection { get { return _connection; } }
        public  IDbTransaction Transaction { get; set; }

        public AdoNetContext(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            _connection = _connectionFactory.Create();
        }

        
        public IDbCommand CreateCommand()
        {
            var cmd = _connection.CreateCommand();
            cmd.Transaction = Transaction;            
            return cmd;
        }

        
        public void Dispose()
        {
            if (Transaction != null)
                Transaction.Dispose();
            _connection.Dispose();
            Transaction = null;
        }

        public virtual void AddParameter(IDbCommand command, string name, DbType dbType, object value, ParameterDirection direction)
        {
            AddParameter(command, name, dbType, 0, direction, false,0,0, String.Empty, DataRowVersion.Default, value);
        }
        public virtual void AddParameter(IDbCommand command, string name, DbType dbType, int size, ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            IDbDataParameter parameter = CreateParameter(command,name, dbType, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, value);
            command.Parameters.Add(parameter);
        }
        protected IDbDataParameter CreateParameter(IDbCommand cmd, string name, DbType dbType, int size, ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            IDbDataParameter param = CreateParameter(cmd, name);
            ConfigureParameter(param, name, dbType, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, value);
            return param;
        }

        protected IDbDataParameter CreateParameter(IDbCommand cmd, string name)
        {
            IDbDataParameter param = cmd.CreateParameter();
            param.ParameterName = name;
            return param;
        }

        protected virtual void ConfigureParameter(IDbDataParameter param, string name, DbType dbType, int size, ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            param.DbType = dbType;
            param.Size = size;
            param.Value = (value == null) ? DBNull.Value : value;
            param.Direction = direction;
            //param.IsNullable = nullable;
            param.SourceColumn = sourceColumn;
            param.SourceVersion = sourceVersion;
        }
    }
}
