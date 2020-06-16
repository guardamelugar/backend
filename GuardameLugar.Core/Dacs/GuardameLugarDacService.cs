using GuardameLugar.Common;
using GuardameLugar.DataAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using GuardameLugar.DataAccess.Base;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using GuardameLugar.Common.Dto;
using System.Collections.Generic;

namespace GuardameLugar.Core.Dacs
{
	public interface IGuardameLugarDacService : ITransactionDacAsync
	{
		Task SaveUser(UserDto userDto);
		Task<bool> MailValidation(string mail);
		Task<LogInDto> LogInUser(string user, string password);
		Task GarageRegister(GarageRegisterDto garageRegisterDto);
		Task<List<LocalidadesDto>> Localidades();
		Task<GarageDto> GetGarageById(int garageId);
		Task<List<GarageDto>> GetGarageByUser(int userId);
		Task<List<GarageDto>> GetGarages(string query);
		Task UpdateGarage(UpdateGarageDto updateGarageDto);
	}

	public class GuardameLugarDacService : IGuardameLugarDacService, IDisposable
	{
		private readonly GuardameLugarDac _guardameLugarDac;

		public GuardameLugarDacService(ILogger<GuardameLugarDacService> logger, IConfiguration configuration)
		{
			string connectionString = configuration["ConnectionStrings:DefaultConnection"];
			_guardameLugarDac = new GuardameLugarDac(connectionString, logger);
		}

		public async Task SaveUser(UserDto userDto)
		{
			await _guardameLugarDac.SaveUser(userDto);
		}

		public async Task<bool> MailValidation(string mail)
		{
			bool result = await _guardameLugarDac.MailValidation(mail);
			return result;
		}

		public async Task<LogInDto> LogInUser(string user, string password)
		{
			return await _guardameLugarDac.LogInUser(user, password);
		}

		public async Task GarageRegister(GarageRegisterDto garageRegisterDto)
		{
			await _guardameLugarDac.GarageRegister(garageRegisterDto);
		}

		public async Task<List<LocalidadesDto>> Localidades()
		{
			List<LocalidadesDto> listaLocalidades = await _guardameLugarDac.Localidades();
			return listaLocalidades;
		}

		public async Task<GarageDto> GetGarageById(int garageId)
		{
			GarageDto garageDto = await _guardameLugarDac.GetGarageById(garageId);
			return garageDto;
		}

		public async Task<List<GarageDto>> GetGarageByUser(int userId)
		{
			List<GarageDto> garageList = await _guardameLugarDac.GetGarageByUser(userId);
			return garageList;
		}
		public async Task<List<GarageDto>> GetGarages(string query)
		{
			List<GarageDto> garageList = await _guardameLugarDac.GetGarages(query);
			return garageList;
		}

		public async Task UpdateGarage(UpdateGarageDto updateGarageDto)
		{
			await _guardameLugarDac.UpdateGarage(updateGarageDto);
		}

		#region --ITransactionDACAsync--
		public async Task<SqlTransaction> BeginTransactionAsync()
		{
			return await _guardameLugarDac.BeginTransactionAsync();
		}

		public void BeginTransaction(SqlTransaction oTransaction)
		{
			throw new NotImplementedException();
		}

		public bool TransactionOpened()
		{
			return _guardameLugarDac.TransactionOpened();
		}

		public void Commit()
		{
			_guardameLugarDac.Commit();
		}

		public void Rollback()
		{
			_guardameLugarDac.Rollback();
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			// Cleanup
		}

		#endregion
	}
}
