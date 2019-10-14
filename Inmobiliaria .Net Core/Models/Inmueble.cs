using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria_.Net_Core.Models
{
	public class Inmueble
	{
        [Key]
        public int IdInmueble { get; set; }
		[Required]
		public string Direccion { get; set; }
		[Required]
		public int Ambientes { get; set; }
		
		public string Tipo { get; set; }
		
		public string Uso { get; set; }
		[Required]
		public int Precio { get; set; }
		[Required]
		public string Disponible { get; set; }

		public int IdPropietario { get; set; }

        public Propietario Propietario { get; set; }
	}
}
