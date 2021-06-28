using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Parcial2_apd2_20180906.Models
{
    public class Cobros
    {
        [Key]
        public int CobroId { get; set; }
        public DateTime Fecha { get; set; }

        public int ClienteId { get; set; }
        public Clientes Cliente { get; set; }

        public decimal Totales { get; set; }
        public double TotalCobro { get; set; }
        
        public string Observaciones { get; set; }

        [ForeignKey("CobroId")]
        public virtual List<CobrosDetalle> Detalle { get; set; }
    }
}
