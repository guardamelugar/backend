
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
		Task GarageRegister(GarageRegisterDto garageRegisterDto);
		Task<List<LocalidadesDto>> Localidades();
		Task<GarageDto> GetGarageById(int garageId);
		Task<List<GarageDto>> GetGarageByUser(int userId);
		Task<List<GarageDto>> GetGarages(string vehiculos, int? localidades);
		Task UpdateGarage(UpdateGarageDto updateGarageDto);
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

		public async Task GarageRegister(GarageRegisterDto garageRegisterDto)
		{
			try
			{
				//validacion de datos completos
				Throws.ThrowIfNull(garageRegisterDto, new BadRequestException("Los parametros son nulos."));
				Throws.ThrowIfNull(garageRegisterDto.user_id, new BadRequestException("usuario esta vacio."));
				Throws.ThrowIfNull(garageRegisterDto.altura_maxima, new BadRequestException("AlturaMaxima esta vacio."));
				Throws.ThrowIfEmpty(garageRegisterDto.coordenadas, new BadRequestException("coordenadas esta vacio."));
				Throws.ThrowIfEmpty(garageRegisterDto.direccion, new BadRequestException("direccion esta vacio."));
				Throws.ThrowIfNull(garageRegisterDto.localidad_garage, new BadRequestException("localidad garage esta vacio."));
				Throws.ThrowIfNull(garageRegisterDto.lugar_autos, new BadRequestException("lugar autos esta vacio."));
				Throws.ThrowIfNull(garageRegisterDto.lugar_bicicletas, new BadRequestException("lugar bicicletas esta vacio."));
				Throws.ThrowIfNull(garageRegisterDto.lugar_camionetas, new BadRequestException("lugar camionetas esta vacio."));
				Throws.ThrowIfNull(garageRegisterDto.lugar_motos, new BadRequestException("lugar motos esta vacio."));
				Throws.ThrowIfEmpty(garageRegisterDto.nombre_garage, new BadRequestException("nombre garage esta vacio."));
				Throws.ThrowIfEmpty(garageRegisterDto.telefono, new BadRequestException("telefono esta vacio."));

				//DataBase
				await _guardameLugarDacService.GarageRegister(garageRegisterDto);


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

		public async Task<List<GarageDto>> GetGarages(string vehiculos, int? localidades)
		{
			//Valido que parametro me ingresa para realizar la consulta
			string query;

			if (string.IsNullOrEmpty(vehiculos) && localidades == null)
			{
				query = "select top 10 * from garages g inner join [guardameLugarDB].[dbo].localidades l on g.localidad_garage = l.localidad_id order by l.nombre_localidad";
			}
			else if (string.IsNullOrEmpty(vehiculos) && localidades > 0)
			{
				query = $"select * from garages g inner join [guardameLugarDB].[dbo].localidades l on g.localidad_garage = l.localidad_id where localidad_garage = {localidades}";
			}
			else if (vehiculos != "" && localidades == null)
			{
				query = (vehiculos.ToLower()) switch
				{
					"autos" => "select * from garages g inner join [guardameLugarDB].[dbo].localidades l on g.localidad_garage = l.localidad_id where lugar_autos > 0 order by l.nombre_localidad;",
					"motos" => "select * from garages g inner join [guardameLugarDB].[dbo].localidades l on g.localidad_garage = l.localidad_id where lugar_motos > 0 order by l.nombre_localidad;",
					"camionetas" => "select * from garages g inner join [guardameLugarDB].[dbo].localidades l on g.localidad_garage = l.localidad_id where lugar_camionetas > 0 order by l.nombre_localidad;",
					"bicicletas" => "select * from garages g inner join [guardameLugarDB].[dbo].localidades l on g.localidad_garage = l.localidad_id where lugar_bicicletas > 0  order by l.nombre_localidad;",
					_ => throw new BadRequestException($"Ningun parametro de {vehiculos} se corresponde con un valor valido"),
				};
			}			
			else
			{
				query = (vehiculos?.ToLower()) switch
				{
					"autos" => $"select * from garages g inner join [guardameLugarDB].[dbo].localidades l on g.localidad_garage = l.localidad_id where localidad_garage = {localidades} and lugar_autos > 0;",
					"motos" => $"select * from garages g inner join [guardameLugarDB].[dbo].localidades l on g.localidad_garage = l.localidad_id where localidad_garage = {localidades} and lugar_motos > 0;",
					"camionetas" => $"select * from garages g inner join [guardameLugarDB].[dbo].localidades l on g.localidad_garage = l.localidad_id where localidad_garage = {localidades} and lugar_camionetas > 0;",
					"bicicletas" => $"select * from garages g inner join [guardameLugarDB].[dbo].localidades l on g.localidad_garage = l.localidad_id where localidad_garage = {localidades} and lugar_bicicletas > 0;",
					_ => throw new BadRequestException($"Ningun parametro de {vehiculos} se corresponde con un valor valido"),
				};
			}

			List<GarageDto> garageList = await _guardameLugarDacService.GetGarages(query);

			return garageList;
		}

		public async Task UpdateGarage(UpdateGarageDto updateGarageDto)
		{
			//Valido los campos necesarios
			Throws.ThrowIfNull(updateGarageDto, new BadRequestException("Los parametros son nulos."));
			Throws.ThrowIfNull(updateGarageDto.altura_maxima, new BadRequestException("AlturaMaxima esta vacio."));
			Throws.ThrowIfEmpty(updateGarageDto.coordenadas, new BadRequestException("coordenadas esta vacio."));
			Throws.ThrowIfEmpty(updateGarageDto.direccion, new BadRequestException("direccion esta vacio."));
			Throws.ThrowIfNull(updateGarageDto.localidad_garage, new BadRequestException("localidad garage esta vacio."));
			Throws.ThrowIfNull(updateGarageDto.lugar_autos, new BadRequestException("lugar autos esta vacio."));
			Throws.ThrowIfNull(updateGarageDto.lugar_bicicletas, new BadRequestException("lugar bicicletas esta vacio."));
			Throws.ThrowIfNull(updateGarageDto.lugar_camionetas, new BadRequestException("lugar camionetas esta vacio."));
			Throws.ThrowIfNull(updateGarageDto.lugar_camionetas, new BadRequestException("lugar camionetas esta vacio."));
			Throws.ThrowIfNull(updateGarageDto.lugar_motos, new BadRequestException("lugar motos esta vacio."));
			Throws.ThrowIfEmpty(updateGarageDto.nombre_garage, new BadRequestException("nombre garage esta vacio."));
			Throws.ThrowIfEmpty(updateGarageDto.telefono, new BadRequestException("telefono esta vacio."));
			Throws.ThrowIfNull(updateGarageDto.garage_id, new BadRequestException("garage_id esta vacio."));

			//hago el update
			await _guardameLugarDacService.UpdateGarage(updateGarageDto);
		}
	}
}
