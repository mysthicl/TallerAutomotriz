using System;
using System.Collections.Generic;

namespace Taller.Entity
{
    public partial class TblReparacion
    {
        public int IdReparacion { get; set; }
        public int? IdUsuario { get; set; }
        public string? DescripcionDeLaReparacion { get; set; }
        public DateTime? FechaDeInicio { get; set; }
        public DateTime? FechaDeFin { get; set; }
        public int? IdCotizacion { get; set; }
        public int? IdHistorialCarro { get; set; }
        public string? Status { get; set; }
        public string? NumberTracking { get; set; }

        public virtual TblCotizacion? IdCotizacionNavigation { get; set; }
        public virtual TblHistorialCarro? IdHistorialCarroNavigation { get; set; }
        public virtual TblUsuario? IdUsuarioNavigation { get; set; }
    }
}
