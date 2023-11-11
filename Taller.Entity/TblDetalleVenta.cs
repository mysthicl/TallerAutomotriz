using System;
using System.Collections.Generic;

namespace Taller.Entity
{
    public partial class TblDetalleVenta
    {
        public int? CantidadVendida { get; set; }
        public int? IdCotizacion { get; set; }
        public int IdProducto { get; set; }
        public decimal? Precio { get; set; }
        public int? Cantidad { get; set; }
        public decimal? Subtotal { get; set; }
        public int? IdVenta { get; set; }
        public int IdDetalleVenta { get; set; }

        public virtual TblProducto IdProductoNavigation { get; set; } = null!;
        public virtual TblVenta? IdVentaNavigation { get; set; }
    }
}
