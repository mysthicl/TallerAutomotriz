using System;
using System.Collections.Generic;

namespace Taller.Entity
{
    public partial class TblCompra
    {
        public TblCompra()
        {
            TblDetalleCompras = new HashSet<TblDetalleCompra>();
        }

        public int IdCompra { get; set; }
        public int? IdProveedor { get; set; }
        public int? IdUsuario { get; set; }
        public int? IdDetalleCompra { get; set; }
        public DateTime? Fecha { get; set; }
        public decimal? TotalDeLaCompra { get; set; }
        public int? IdProducto { get; set; }
        public string? NumeroCompra { get; set; }

        public virtual TblProducto? IdProductoNavigation { get; set; }
        public virtual TblProveedor? IdProveedorNavigation { get; set; }
        public virtual TblUsuario? IdUsuarioNavigation { get; set; }
        public virtual ICollection<TblDetalleCompra> TblDetalleCompras { get; set; }
    }
}
