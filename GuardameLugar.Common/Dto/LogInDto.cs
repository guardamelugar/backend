﻿
namespace GuardameLugar.Common.Dto
{
	public class LogInDto
	{
		public int user_id { get; set; }
		public string nombre { get; set; }
		public string apellido { get; set; }
		public string telefono { get; set; }
		public string mail { get; set; }
		public int rol { get; set; }
		public string contraseña { get; set; }
	}
}
