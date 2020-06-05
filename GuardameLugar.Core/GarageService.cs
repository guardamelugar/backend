
using GuardameLugar.Common.Dto;
using GuardameLugar.Common.Exceptions;
using GuardameLugar.Common.Helpers;
using GuardameLugar.Core.Dacs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace GuardameLugar.Core
{
	public interface IGarageService
	{
		Task GarageRegister(GarageDto garageDto);
		Task<List<LocalidadesDto>> Localidades();
		Task<GarageDto> GetGarageById(int garageId);
		Task<List<GarageDto>> GetGarageByUser(int userId);
	}
	public class GarageService : IGarageService
	{
		private readonly ILogger<GarageService> _logger;
		private readonly IGuardameLugarDacService _guardameLugarDacService;

		public GarageService(ILogger<GarageService> logger, IGuardameLugarDacService guardameLugarDacService)
		{
			_logger = logger;
			_guardameLugarDacService = guardameLugarDacService;
		}

		public async Task GarageRegister(GarageDto garageDto)
		{
			try
			{
				//validacion de datos completos
				Throws.ThrowIfNull(garageDto, new BadRequestException("Los parametros son nulos."));
				Throws.ThrowIfNull(garageDto.user_id, new BadRequestException("usuario esta vacio."));
				Throws.ThrowIfNull(garageDto.altura_maxima, new BadRequestException("AlturaMaxima esta vacio."));
				Throws.ThrowIfEmpty(garageDto.coordenadas, new BadRequestException("coordenadas esta vacio."));
				Throws.ThrowIfEmpty(garageDto.direccion, new BadRequestException("direccion esta vacio."));
				Throws.ThrowIfNull(garageDto.localidad_garage, new BadRequestException("localidad garage esta vacio."));
				Throws.ThrowIfNull(garageDto.lugar_autos, new BadRequestException("lugar autos esta vacio."));
				Throws.ThrowIfNull(garageDto.lugar_bicicletas, new BadRequestException("lugar bicicletas esta vacio."));
				Throws.ThrowIfNull(garageDto.lugar_camionetas, new BadRequestException("lugar camionetas esta vacio."));
				Throws.ThrowIfNull(garageDto.lugar_camionetas, new BadRequestException("lugar camionetas esta vacio."));
				Throws.ThrowIfNull(garageDto.lugar_motos, new BadRequestException("lugar motos esta vacio."));
				Throws.ThrowIfEmpty(garageDto.nombre_garage, new BadRequestException("nombre garage esta vacio."));
				Throws.ThrowIfEmpty(garageDto.telefono, new BadRequestException("telefono esta vacio."));

				//DataBase
				await _guardameLugarDacService.GarageRegister(garageDto);


			}
			catch (Exception e)
			{
				_logger.LogError(e, GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
				throw;
			}


		}

		public async Task<List<LocalidadesDto>> Localidades()
		{
			List<LocalidadesDto> listaLocalidades = await _guardameLugarDacService.Localidades();
			return listaLocalidades;
		}

		public async Task<GarageDto> GetGarageById(int garageId)
		{
			//valido que ID no sea nulo
			Throws.ThrowIfNotPositive(garageId, new BadRequestException("ID no puede ser negativo."));

			GarageDto garageDto = await _guardameLugarDacService.GetGarageById(garageId);

			//valido que exista el garage
			Throws.ThrowIfNull(garageDto, new NotFoundException("No se encontro el Garage."));

			return garageDto;
		}

		public async Task<List<GarageDto>> GetGarageByUser(int userId)
		{
			//valido que ID no sea nulo
			Throws.ThrowIfNotPositive(userId, new BadRequestException("ID no puede ser negativo."));

			List<GarageDto> garageList = await _guardameLugarDacService.GetGarageByUser(userId);

			//valido que exista el garage
			Throws.ThrowIfNull(garageList, new NotFoundException("No se encontraron Garages para el usuario."));

			return garageList;
		}
	}
}
