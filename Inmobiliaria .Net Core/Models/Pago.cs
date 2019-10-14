using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria_.Net_Core.Models
{
    public class Pago
    {
		[Key]
        public int IdPago { get; set; }
		[Required]
		public int NroPago { get; set; }
		[Required]
		public int IdAlquiler { get; set; }
        public Alquiler Alquiler { get; set; }
		[Required]
		public string Fecha { get; set; }
		[Required]
		public int Importe { get; set; }
    }
}
