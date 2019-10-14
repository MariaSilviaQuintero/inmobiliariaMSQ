using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria_.Net_Core.Models
{
	public class Alquiler
	{
        [Key]
        public int IdAlquiler { get; set; }
		[Required]
		public int Importe { get; set; }
		[Required]
		public string FechaInicio { get; set; }
		[Required]
		public string FechaFin { get; set; }
		[Required]
		public int IdInquilino { get; set; }
		[Required]
		public int IdInmueble { get; set; }

        public Inquilino Inquilino { get; set; }

		public Inmueble Inmueble { get; set; }

		
	}
}
