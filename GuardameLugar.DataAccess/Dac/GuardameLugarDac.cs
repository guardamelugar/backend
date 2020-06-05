using GuardameLugar.Common;
using Microsoft.Extensions.Logging;
using GuardameLugar.DataAccess.Base;
using GuardameLugar.DataAccess.Extensions;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using GuardameLugar.DataAccess.Helpers;
using GuardameLugar.Common.Dto;
using System.Collections.Generic;

namespace GuardameLugar.DataAccess
{
	public class GuardameLugarDac : BaseDacAsync
	{
		private static readonly string _classFullName = MethodBase.GetCurrentMethod().DeclaringType?.ToString();
		private readonly ILogger _logger;

		public GuardameLugarDac(string connectionString, ILogger logger) : base(connectionString)
		{
			try
			{
				_logger = logger ?? throw new ArgumentNullException(nameof(logger));

				base.OpenConnectionAsync().Wait();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
				throw new AggregateException(_classFullName + ".GuardameLugarDac(string connectionString) (ctor)", ex);
			}
		}

		#region Client
		public async Task SaveUser(UserDto userDto)
		{

			using (SqlCommand oCommand = await base.GetCommandAsync())
			{
				try
				{
					oCommand.CommandType = CommandType.Text;
					oCommand.CommandText = @"insert into usuarios (nombre, apellido, telefono, mail, contraseña, rol) values (@nombre, @apellido, @telefono, @mail, @contraseña, @rol);";

					oCommand.AddParameter("nombre", DbType.String, userDto.nombre);
					oCommand.AddParameter("apellido", DbType.String, userDto.apellido);
					oCommand.AddParameter("telefono", DbType.String, userDto.telefono);
					oCommand.AddParameter("mail", DbType.String, userDto.mail);
					oCommand.AddParameter("contraseña", DbType.String, userDto.contraseña);
					oCommand.AddParameter("rol", DbType.Int32, userDto.rol);

					await oCommand.ExecuteNonQueryAsync();

				}
				catch (Exception ex)
				{
					_logger.LogError(ex, GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
					throw new AggregateException(_classFullName + ".ValidMainCard(long accountId)", ex);
				}
				finally
				{
					if (!TransactionOpened())
						base.CloseCommand();
				}
			}
		}

		public async Task<bool> MailValidation(string mail)
		{
			SqlDataReader reader;

			using (SqlCommand oCommand = await base.GetCommandAsync())
			{
				try
				{
					oCommand.CommandType = CommandType.Text;
					oCommand.CommandText = @"select mail from usuarios where mail = @mail";

					oCommand.AddParameter("mail", DbType.String, mail);

					IAsyncResult asyncResult = ExecuteAsync(oCommand, "MailValidation");
					reader = oCommand.EndExecuteReader(asyncResult);

					if (reader.HasRows)
					{
						return true;
					}
					else
					{
						return false;
					}

				}
				catch (Exception ex)
				{
					_logger.LogError(ex, GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
					throw new AggregateException(_classFullName + ".ValidMainCard(long accountId)", ex);
				}
				finally
				{
					if (!TransactionOpened())
						base.CloseCommand();
				}
			}
		}

		public async Task<LogInDto> LogInUser(string user, string password)
		{
			LogInDto objUser = null;
			SqlDataReader reader = null;
			using (SqlCommand oCommand = await base.GetCommandAsync())
			{
				try
				{
					oCommand.CommandType = CommandType.Text;
					oCommand.CommandText = @"SELECT * FROM usuarios WHERE mail = @user and contraseña = @password";

					oCommand.AddParameter("user", DbType.String, user);
					oCommand.AddParameter("password", DbType.String, password);

					IAsyncResult asyncResult = ExecuteAsync(oCommand, "LogInUser");
					reader = oCommand.EndExecuteReader(asyncResult);

					while (await reader.ReadAsync())
					{
						objUser = ModelBuilderHelper.BuildUserData(reader);
					}
					return objUser;
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
					throw new AggregateException(_classFullName + ".LogInUser(string user, string password)", ex);
				}
				finally
				{
					if (reader != null && !reader.IsClosed)
						reader.Close();
					if (!TransactionOpened())
						base.CloseCommand();
				}
			}
		}

		#endregion

		#region Garage
		public async Task GarageRegister(GarageDto garageDto)
		{

			using (SqlCommand oCommand = await base.GetCommandAsync())
			{
				try
				{
					oCommand.CommandType = CommandType.Text;
					oCommand.CommandText = @"INSERT INTO GARAGES (altura_maxima, coordenadas, telefono, direccion, localidad_garage, lugar_autos, lugar_bicicletas, lugar_camionetas, lugar_motos, nombre_garage) 
                                             VALUES	(@altura_maxima, @coordenadas, @telefono, @direccion, @localidad_garage, @lugar_autos, @lugar_bicicletas, @lugar_camionetas, @lugar_motos, @nombre_garage);
											SELECT SCOPE_IDENTITY()";


					oCommand.AddParameter("altura_maxima", DbType.Decimal, garageDto.altura_maxima);
					oCommand.AddParameter("coordenadas", DbType.String, garageDto.coordenadas);
					oCommand.AddParameter("telefono", DbType.String, garageDto.telefono);
					oCommand.AddParameter("direccion", DbType.String, garageDto.direccion);
					oCommand.AddParameter("localidad_garage", DbType.Int32, garageDto.localidad_garage);
					oCommand.AddParameter("lugar_autos", DbType.Int32, garageDto.lugar_autos);
					oCommand.AddParameter("lugar_bicicletas", DbType.Int32, garageDto.lugar_bicicletas);
					oCommand.AddParameter("lugar_camionetas", DbType.Int32, garageDto.lugar_camionetas);
					oCommand.AddParameter("lugar_motos", DbType.Int32, garageDto.lugar_motos);
					oCommand.AddParameter("nombre_garage", DbType.String, garageDto.nombre_garage);

					int garage_id = Convert.ToInt32(oCommand.ExecuteScalar());

					oCommand.CommandText = @"INSERT INTO [dbo].[garage_por_usuario] (user_id, garage_id) VALUES 
											((select user_id from [dbo].[usuarios] where user_id = @user_id), (select [garage_id] from [dbo].[garages] where [garage_id] = @garage_id));";

					oCommand.AddParameter("user_id", DbType.Int32, garageDto.user_id);
					oCommand.AddParameter("garage_id", DbType.Int32, garage_id);

					await oCommand.ExecuteNonQueryAsync();

				}
				catch (Exception ex)
				{
					_logger.LogError(ex, GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
					throw new AggregateException(_classFullName + ".ValidMainCard(long accountId)", ex);
				}
				finally
				{
					if (!TransactionOpened())
						base.CloseCommand();
				}
			}
		}

		public async Task<List<LocalidadesDto>> Localidades()
		{
			List<LocalidadesDto> listaLocalidades = new List<LocalidadesDto>();
			SqlDataReader reader = null;
			using (SqlCommand oCommand = await base.GetCommandAsync())
			{
				try
				{
					oCommand.CommandType = CommandType.Text;
					oCommand.CommandText = @"SELECT * FROM localidades;";

					IAsyncResult asyncResult = ExecuteAsync(oCommand, "localidades");
					reader = oCommand.EndExecuteReader(asyncResult);

					while (await reader.ReadAsync())
					{
						listaLocalidades.Add(ModelBuilderHelper.BuildLocalidadesData(reader));
					}
					return listaLocalidades;
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
					throw new AggregateException(_classFullName + ".Localidades()", ex);
				}
				finally
				{
					if (reader != null && !reader.IsClosed)
						reader.Close();
					if (!TransactionOpened())
						base.CloseCommand();
				}
			}
		}

		public async Task<GarageDto> GetGarageById(int garageId)
		{
			GarageDto garageDto = null;
			SqlDataReader reader = null;
			using (SqlCommand oCommand = await base.GetCommandAsync())
			{
				try
				{
					oCommand.CommandType = CommandType.Text;
					oCommand.CommandText = @"SELECT * FROM garages where garage_id = @garage_id;";

					oCommand.AddParameter("garage_id", DbType.Int32, garageId);

					IAsyncResult asyncResult = ExecuteAsync(oCommand, "GetGarageById");
					reader = oCommand.EndExecuteReader(asyncResult);

					while (await reader.ReadAsync())
					{
						garageDto = ModelBuilderHelper.BuildGaragesData(reader);
					}
					return garageDto;
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
					throw new AggregateException(_classFullName + ".Localidades()", ex);
				}
				finally
				{
					if (reader != null && !reader.IsClosed)
						reader.Close();
					if (!TransactionOpened())
						base.CloseCommand();
				}
			}
		}

		public async Task<List<GarageDto>> GetGarageByUser(int userId)
		{
			List<GarageDto> garageList = new List<GarageDto>();
			SqlDataReader reader = null;
			using (SqlCommand oCommand = await base.GetCommandAsync())
			{
				try
				{
					oCommand.CommandType = CommandType.Text;
					oCommand.CommandText = @"select * From garages where garage_id in (select garage_id from garage_por_usuario where user_id = @user_id);";

					oCommand.AddParameter("user_id", DbType.Int32, userId);

					IAsyncResult asyncResult = ExecuteAsync(oCommand, "GetGarageByUser");
					reader = oCommand.EndExecuteReader(asyncResult);

					while (await reader.ReadAsync())
					{
						garageList.Add(ModelBuilderHelper.BuildGaragesData(reader));
					}
					return garageList;
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
					throw new AggregateException(_classFullName + ".GetGarageByUser(int userId)", ex);
				}
				finally
				{
					if (reader != null && !reader.IsClosed)
						reader.Close();
					if (!TransactionOpened())
						base.CloseCommand();
				}
			}
		}

		#endregion

		#region ExecutePArameters
		private IAsyncResult ExecuteAsync(SqlCommand oCommand, string methods = "*")
		{
			var watch = System.Diagnostics.Stopwatch.StartNew();
			int count = 0;
			IAsyncResult asyncResult = oCommand.BeginExecuteReader();
			while (!asyncResult.IsCompleted)
			{
				Thread.Sleep(100);
				count++;
				if (count == 1000000)
				{
					watch.Stop();
					var elapsedTime = watch.ElapsedMilliseconds;
					_logger.LogInformation("The method '" + methods + "' delay " + elapsedTime.ToString() + " miliseconds.", GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
					throw new AggregateException("The method '" + methods + "' it took longer than parameterized.");
				}
			}
			return asyncResult;
		}
		#endregion
	}
}



