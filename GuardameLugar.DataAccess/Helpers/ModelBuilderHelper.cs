

using GuardameLugar.Common.Dto;
using System.Data;

namespace GuardameLugar.DataAccess.Helpers
{
	internal static class ModelBuilderHelper
	{
		//aca van los mapeos de la base de datos

		internal static LogInDto BuildUserData(IDataReader reader)
		{
			LogInDto User = new LogInDto();
			User.user_id = int.Parse(reader["user_id"].ToString());
			User.nombre = (reader["nombre"].ToString());
			User.apellido = (reader["apellido"].ToString());
			User.mail = (reader["mail"].ToString());
			User.rol = int.Parse(reader["rol"].ToString());
			User.telefono = (reader["telefono"].ToString());
			User.contraseña = (reader["contraseña"].ToString());

			return User;
		}

		internal static LocalidadesDto BuildLocalidadesData(IDataReader reader)
		{
			LocalidadesDto localidades = new LocalidadesDto();
			localidades.localidad_id = int.Parse(reader["localidad_id"].ToString());
			localidades.nombre_localidad = (reader["nombre_localidad"].ToString());
			return localidades;
		}

		internal static GarageDto BuildGaragesData(IDataReader reader)
		{
			GarageDto garageDto = new GarageDto();
			garageDto.altura_maxima = int.Parse(reader["altura_maxima"].ToString());
			garageDto.coordenadas = (reader["coordenadas"].ToString());
			garageDto.direccion = (reader["direccion"].ToString());
			garageDto.garage_id = int.Parse(reader["garage_id"].ToString());
			garageDto.localidad_garage = int.Parse(reader["localidad_garage"].ToString());
			garageDto.lugar_autos = int.Parse(reader["lugar_autos"].ToString());
			garageDto.lugar_bicicletas = int.Parse(reader["lugar_bicicletas"].ToString());
			garageDto.lugar_camionetas = int.Parse(reader["lugar_camionetas"].ToString());
			garageDto.lugar_motos = int.Parse(reader["lugar_motos"].ToString());
			garageDto.nombre_garage = (reader["nombre_garage"].ToString());
			garageDto.telefono = (reader["telefono"].ToString());
			return garageDto;
		}

	}
}
