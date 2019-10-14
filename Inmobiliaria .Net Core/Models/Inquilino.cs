using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria_.Net_Core.Models
{
	public class Inquilino
	{
        [Key]
        public int IdInquilino { get; set; }
        [Required]
        public string Dni { get; set; }
        [Required]
        public string Apellido { get; set; }
        [Required]
		public string Nombre { get; set; }
        public string Direccion { get; set; }
		public string Telefono { get; set; }
	}
}
