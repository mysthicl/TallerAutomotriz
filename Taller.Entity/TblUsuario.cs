using System;
using System.Collections.Generic;

namespace Taller.Entity
{
    public partial class TblUsuario
    {
        public TblUsuario()
        {
            TblCompras = new HashSet<TblCompra>();
            TblCotizacions = new HashSet<TblCotizacion>();
            TblReparacions = new HashSet<TblReparacion>();
            TblVenta = new HashSet<TblVenta>();
        }

        public int IdUsuario { get; set; }
        public string? NombreDeUsuario { get; set; }
        public string? Contrasena { get; set; }
        public string? UrlFoto { get; set; }
        public string? NombreFoto { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public int? IdRol { get; set; }

        public virtual TblRol? IdRolNavigation { get; set; }
        public virtual ICollection<TblCompra> TblCompras { get; set; }
        public virtual ICollection<TblCotizacion> TblCotizacions { get; set; }
        public virtual ICollection<TblReparacion> TblReparacions { get; set; }
        public virtual ICollection<TblVenta> TblVenta { get; set; }
    }
}
