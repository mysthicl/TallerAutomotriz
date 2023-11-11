using System;
using System.Collections.Generic;

namespace Taller.Entity
{
    public partial class TblVenta
    {
        public TblVenta()
        {
            TblDetalleVenta = new HashSet<TblDetalleVenta>();
        }

        public int IdVenta { get; set; }
        public int? IdUsuario { get; set; }
        public DateTime? Fecha { get; set; }
        public decimal? TotalDeLaVenta { get; set; }
        public string? DocumentoCliente { get; set; }
        public string? NombreCliente { get; set; }
        public int? Telefono { get; set; }
        public string? Direccion { get; set; }
        public string? NumeroVenta { get; set; }

        public virtual TblUsuario? IdUsuarioNavigation { get; set; }
        public virtual ICollection<TblDetalleVenta> TblDetalleVenta { get; set; }
    }
}
