using System;
using System.Collections.Generic;

namespace Taller.Entity
{
    public partial class TblProveedor
    {
        public TblProveedor()
        {
            TblCompras = new HashSet<TblCompra>();
        }

        public int IdProveedor { get; set; }
        public string? Nombre { get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }

        public virtual ICollection<TblCompra> TblCompras { get; set; }
    }
}
