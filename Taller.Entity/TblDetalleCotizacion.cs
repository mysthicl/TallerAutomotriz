using System;
using System.Collections.Generic;

namespace Taller.Entity
{
    public partial class TblDetalleCotizacion
    {
        public int IdDetalleCotizacion { get; set; }
        public decimal? Precio { get; set; }
        public int? Cantidad { get; set; }
        public decimal? Subtotal { get; set; }
        public int? IdCotizacion { get; set; }
        public int? IdProducto { get; set; }

        public virtual TblCotizacion? IdCotizacionNavigation { get; set; }
        public virtual TblProducto? IdProductoNavigation { get; set; }
    }
}
