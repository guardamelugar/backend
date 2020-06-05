using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace GuardameLugar.DataAccess.Base
{
    public abstract class AbstractBaseDacAsync
    {
        protected virtual string ConnectionString { get; set; }

        protected virtual string Schema { get; set; }

        protected virtual Task OpenConnectionAsync()
        {
            throw new NotImplementedException("OpenConnectionAsync()");
        }
        protected virtual void CloseConnection()
        {
            throw new NotImplementedException("CloseConnection()");
        }
        protected virtual Task<SqlCommand> GetCommandAsync()
        {
            throw new NotImplementedException("GetCommandAsync()");
        }

        protected virtual Task OpenCommandAsync()
        {
            throw new NotImplementedException("OpenCommandAsync()");
        }

        protected virtual void CloseCommand()
        {
            throw new NotImplementedException("CloseCommand()");
        }

        protected virtual SqlDataAdapter CreateDataAdapter(string sql)
        {
            throw new NotImplementedException("CreateDataAdapter(string sql)");
        }

        protected virtual SqlDataAdapter CreateDataAdapter()
        {
            throw new NotImplementedException("CreateDataAdapter()");
        }
    }
}
