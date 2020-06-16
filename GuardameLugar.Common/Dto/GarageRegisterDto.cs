
namespace GuardameLugar.Common.Dto
{
	public class GarageRegisterDto
	{
		public int user_id { get; set; }
		public string nombre_garage { get; set; }
		public string direccion { get; set; }
		public string coordenadas { get; set; }
		public int localidad_garage { get; set; }
		public string telefono { get; set; }
		public int lugar_autos { get; set; }
		public int lugar_motos { get; set; }
		public int lugar_camionetas { get; set; }
		public int lugar_bicicletas { get; set; }
		public decimal altura_maxima { get; set; }
	}
}
