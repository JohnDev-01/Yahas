using System;
using System.Collections.Generic;
using System.Text;

namespace Yahas.Models
{
    public class Megresos
    {
        public string IdEgreso { get; set; }
        public string Concepto { get; set; }
        public string Fecha { get; set; }
        public string Idusuario { get; set; }
        public string Idcategoria { get; set; }
        public string Monto { get; set; }
    }
}
