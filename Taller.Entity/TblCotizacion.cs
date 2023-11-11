using System;
using System.Collections.Generic;

namespace Taller.Entity
{
    public partial class TblCotizacion
    {
        public TblCotizacion()
        {
            TblDetalleCotizacions = new HashSet<TblDetalleCotizacion>();
            TblReparacions = new HashSet<TblReparacion>();
        }

        public int IdCotizacion { get; set; }
        public string? Cotizacion { get; set; }
        public DateTime? FechaCotizacion { get; set; }
        public int? IdUsuario { get; set; }
        public int? IdProducto { get; set; }
        public string? NumeroCotizacion { get; set; }
        public decimal? TotalDeLaCotizacion { get; set; }

        public virtual TblProducto? IdProductoNavigation { get; set; }
        public virtual TblUsuario? IdUsuarioNavigation { get; set; }
        public virtual ICollection<TblDetalleCotizacion> TblDetalleCotizacions { get; set; }
        public virtual ICollection<TblReparacion> TblReparacions { get; set; }
    }
}
