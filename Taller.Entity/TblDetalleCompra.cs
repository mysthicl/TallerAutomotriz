﻿using System;
using System.Collections.Generic;

namespace Taller.Entity
{
    public partial class TblDetalleCompra
    {
        public int IdDetalleCompra { get; set; }
        public int? CantidadCompra { get; set; }
        public decimal? Precio { get; set; }
        public int? Cantidad { get; set; }
        public int? IdCompra { get; set; }
        public decimal? Subtotal { get; set; }
        public int? IdProducto { get; set; }

        public virtual TblCompra? IdCompraNavigation { get; set; }
        public virtual TblProducto? IdProductoNavigation { get; set; }
    }
}
