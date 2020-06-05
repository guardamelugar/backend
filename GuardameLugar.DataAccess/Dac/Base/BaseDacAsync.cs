using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Threading.Tasks;

namespace GuardameLugar.DataAccess.Base
{
	public interface ITransactionDacAsync
	{
		Task<SqlTransaction> BeginTransactionAsync();
		void BeginTransaction(SqlTransaction oTransaction);
		bool TransactionOpened();
		void Commit();
		void Rollback();
	}

	public class BaseDacAsync : AbstractBaseDacAsync, ITransactionDacAsync, IDisposable
	{
		#region --Attributes--

		private static readonly string _classFullName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
		private readonly string _connectionString;
		private SqlCommand _oDbCommand;
		private SqlConnection _oDbConnection;
		private SqlTransaction _oDbTransaction;

		#endregion --Attributes--

		#region --Properties--

		protected override string ConnectionString
		{

			get; ///{ return _connectionString; }
			set; ///{ _connectionString = value; }
		}

		#endregion --Properties--

		#region --Constructors & Destructors--

		protected BaseDacAsync(string connectionString)
		{
			try
			{
				_connectionString = connectionString;
			}
			catch (Exception e)
			{
				throw new AggregateException(_classFullName + ".BaseDACAsync() [Constructor]", e);
			}
		}

		#endregion --Constructors & Destructors--

		#region --Methods--

		#region --Transaction ITransactionDAC members--

		public async Task<SqlTransaction> BeginTransactionAsync()
		{
			_oDbTransaction = (await GetConnectionAsync()).BeginTransaction();
			return _oDbTransaction;
		}

		public void BeginTransaction(SqlTransaction oTransaction)
		{
			_oDbTransaction = oTransaction;
			CloseConnection();
			_oDbConnection = oTransaction.Connection;
		}

		public void Commit()
		{
			if (_oDbTransaction != null)
				_oDbTransaction.Commit();
			_oDbTransaction = null;
			CloseCommand();
		}

		public void Rollback()
		{
			if (_oDbTransaction != null)
				_oDbTransaction.Rollback();
			_oDbTransaction = null;
			CloseCommand();
		}

		public void Rollback(string transactionName)
		{
			if (_oDbTransaction != null)
				_oDbTransaction.Rollback();
			_oDbTransaction = null;
			CloseCommand();
		}

		public bool TransactionOpened()
		{
			return _oDbTransaction != null;
		}

		#endregion --Transaction--

		#region --Connection--

		protected override async Task OpenConnectionAsync()
		{
			try
			{
				if (_oDbConnection == null)
					await SetConnectionAsync();
				else if (_oDbConnection.State == ConnectionState.Closed)
					await _oDbConnection.OpenAsync();
			}
			catch (Exception e)
			{
				throw new AggregateException(_classFullName + ".OpenConnectionAsync()", e);
			}
		}

		protected override void CloseConnection()
		{
			if (_oDbConnection != null && _oDbConnection.State != ConnectionState.Closed)
			{
				//(_oDbConnection.State != ConnectionState.Closed)

				try
				{
					_oDbConnection.Close();
				}
				catch (Exception e)
				{
					throw new AggregateException(_classFullName + ".CloseConnection()", e);
				}

			}
		}

		protected override async Task<SqlCommand> GetCommandAsync()
		{
			try
			{
				await OpenCommandAsync();
				_oDbCommand.Parameters.Clear();
				return _oDbCommand;
			}
			catch (Exception e)
			{
				throw new AggregateException(_classFullName + ".GetCommand()", e);
			}
		}

		protected override async Task OpenCommandAsync()
		{
			if (_oDbCommand == null)
				_oDbCommand = (await GetConnectionAsync()).CreateCommand();
			else
			{
				if (_oDbCommand.Connection.State == ConnectionState.Closed)
				{
					await SetConnectionAsync();
					_oDbCommand.Connection = _oDbConnection;
				}
			}
			if (_oDbTransaction != null)
				_oDbCommand.Transaction = _oDbTransaction;
		}

		protected override void CloseCommand()
		{
			if (_oDbCommand != null)
			{
				_oDbCommand.Connection.Close();
				_oDbCommand.Dispose();
				_oDbCommand = null;
				CloseConnection();
			}
		}

		protected override SqlDataAdapter CreateDataAdapter(string sql)
		{
			try
			{
				return new SqlDataAdapter(sql, _connectionString);
			}
			catch (Exception ex)
			{
				throw new AggregateException(_classFullName + ".CreateDataAdapter(string sql)", ex);
			}
		}

		protected override SqlDataAdapter CreateDataAdapter()
		{
			try
			{
				return new SqlDataAdapter();
			}
			catch (Exception ex)
			{
				throw new AggregateException(_classFullName + ".CreateDataAdapter()", ex);
			}
		}

		private async Task SetConnectionAsync()
		{
			try
			{
				_oDbConnection = new SqlConnection(_connectionString);
				try
				{
					await _oDbConnection.OpenAsync();
				}
				catch (Exception ex1)
				{
					throw new AggregateException(_classFullName + ".SetConnection()", ex1);
				}
			}
			catch (Exception ex2)
			{
				throw new AggregateException(_classFullName + ".SetConnection()", ex2);
			}
		}

		private async Task<SqlConnection> GetConnectionAsync()
		{
			await OpenConnectionAsync();
			return _oDbConnection;
		}

		#endregion

		#endregion --Methods--

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			// Cleanup
		}
	}
}
