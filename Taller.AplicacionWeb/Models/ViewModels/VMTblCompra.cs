namespace Taller.AplicacionWeb.Models.ViewModels
{
    public class VMTblCompra
    {
        public int IdCompra { get; set; }
        public string? NumeroCompra { get; set; }
        public int? IdUsuario { get; set; }
        public string? Usuario { get; set; }
        public string? TotalDeLaCompra { get; set; }
        public string? Fecha { get; set; }
        public decimal? subtotal { get; set; }
        public double? iva { get; set; }

		public virtual ICollection<VMTblDetalleCompra> TblDetalleCompras { get; set; }
	}
}
