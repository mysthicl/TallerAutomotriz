using System;
using System.Collections.Generic;

namespace Taller.Entity
{
    public partial class TblProducto
    {
        public TblProducto()
        {
            TblCompras = new HashSet<TblCompra>();
            TblCotizacions = new HashSet<TblCotizacion>();
            TblDetalleCompras = new HashSet<TblDetalleCompra>();
            TblDetalleCotizacions = new HashSet<TblDetalleCotizacion>();
            TblDetalleVenta = new HashSet<TblDetalleVenta>();
        }

        public string? CodigoProducto { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public decimal? Precio { get; set; }
        public int? CantidadEnStock { get; set; }
        public int IdProducto { get; set; }
        public string? UrlImagen { get; set; }
        public string? NombreImagen { get; set; }
        public string? Valor { get; set; }
        public decimal? Ganancia { get; set; }

        public virtual ICollection<TblCompra> TblCompras { get; set; }
        public virtual ICollection<TblCotizacion> TblCotizacions { get; set; }
        public virtual ICollection<TblDetalleCompra> TblDetalleCompras { get; set; }
        public virtual ICollection<TblDetalleCotizacion> TblDetalleCotizacions { get; set; }
        public virtual ICollection<TblDetalleVenta> TblDetalleVenta { get; set; }
    }
}
