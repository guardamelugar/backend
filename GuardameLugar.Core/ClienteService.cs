using GuardameLugar.Common;
using GuardameLugar.Common.Dto;
using GuardameLugar.Common.Exceptions;
using GuardameLugar.Common.Helpers;
using GuardameLugar.Core.Dacs;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace GuardameLugar.Core
{
	public interface IClienteService
	{
		Task SaveUser(UserDto userDto);
		Task<LogInDto> LogInUser(string user, string password);
	}
	public class ClienteService : IClienteService
	{
		private readonly IGuardameLugarDacService _guardameLugarDacService;
		private readonly ILogger<ClienteService> _logger;

		public ClienteService(ILogger<ClienteService> logger, IGuardameLugarDacService guardameLugarDacService)
		{
			_logger = logger;
			_guardameLugarDacService = guardameLugarDacService;
		}
		
		public async Task SaveUser(UserDto userDto)
		{
			try
			{
				//validacion de datos completos
				Throws.ThrowIfNull(userDto, new BadRequestException("Los parametros son nulos."));
				Throws.ThrowIfEmpty(userDto.apellido, new BadRequestException("Apellido esta vacio."));
				Throws.ThrowIfEmpty(userDto.contraseña, new BadRequestException("contraseña esta vacio."));
				Throws.ThrowIfEmpty(userDto.mail, new BadRequestException("mail esta vacio."));
				Throws.ThrowIfEmpty(userDto.nombre, new BadRequestException("nombre esta vacio."));
				Throws.ThrowIfNull(userDto.rol, new BadRequestException("rol esta vacio."));
				Throws.ThrowIfEmpty(userDto.telefono, new BadRequestException("telefono esta vacio."));

				//validar unico mail
				string mail = userDto.mail;
				bool mailvalidator = await _guardameLugarDacService.MailValidation(mail);

				if (mailvalidator)
				{
					throw new BadRequestException("Este mail ya fue registrado.");
				}

				//encriptacion
				userDto.contraseña = EncriptingPass.GetSHA256(userDto.contraseña);

				//DataBase
				await _guardameLugarDacService.SaveUser(userDto);

			}
			catch (Exception e)
			{
				_logger.LogError(e, GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
				throw;
			}
			
		}

		public async Task<LogInDto> LogInUser(string user, string password)
		{
			try
			{
				Throws.ThrowIfEmpty(user, new BadRequestException("user esta vacio."));
				Throws.ThrowIfEmpty(password, new BadRequestException("Pasword esta vacio."));

				if (password.Length <= 15)
				{
					password = EncriptingPass.GetSHA256(password);
				}				

				var userData = await _guardameLugarDacService.LogInUser(user, password);
				Throws.ThrowIfNull(userData, new NotFoundException($"No se encontro el usuario: {user}."));

				return userData;

			}
			catch (Exception e)
			{
				_logger.LogError(e, GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
				throw;
			}
		}
	}
}
