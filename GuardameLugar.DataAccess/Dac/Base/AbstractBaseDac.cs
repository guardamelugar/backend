using System;
using System.Data;

namespace GuardameLugar.DataAccess.Base
{
	public abstract class AbstractBaseDac
	{
		protected virtual string ConnectionString { get; set; }

		protected virtual string Schema { get; set; }

		protected virtual void OpenConnection()
		{
			throw new NotImplementedException("OpenConnection()");
		}
		protected virtual void CloseConnection()
		{
			throw new NotImplementedException("CloseConnection()");
		}
		protected virtual IDbCommand GetCommand()
		{
			throw new NotImplementedException("GetCommand()");
		}

		protected virtual void OpenCommand()
		{
			throw new NotImplementedException("OpenCommand()");
		}

		protected virtual void CloseCommand()
		{
			throw new NotImplementedException("CloseCommand()");
		}

		protected virtual IDbDataAdapter CreateDataAdapter(string sql)
		{
			throw new NotImplementedException("CreateDataAdapter(string sql)");
		}

		protected virtual IDbDataAdapter CreateDataAdapter()
		{
			throw new NotImplementedException("CreateDataAdapter()");
		}
	}
}
