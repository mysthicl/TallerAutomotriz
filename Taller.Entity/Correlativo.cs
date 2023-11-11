using System;
using System.Collections.Generic;

namespace Taller.Entity
{
    public partial class Correlativo
    {
        public int IdCorrelativo { get; set; }
        public int? NumeroCorrelativo { get; set; }
        public int? CantidadNumero { get; set; }
        public string? Tipo { get; set; }
    }
}
