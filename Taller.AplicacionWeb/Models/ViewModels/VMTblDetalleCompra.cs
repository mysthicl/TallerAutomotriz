namespace Taller.AplicacionWeb.Models.ViewModels
{
	public class VMTblDetalleCompra
	{
		public int IdDetalleCompra { get; set; }
		public int? IdCotizacion { get; set; }
		public int IdProducto { get; set; }
		public string? Precio { get; set; }
        public string? DescripcionProducto { get; set; }
        public int? Cantidad { get; set; }
		public string? Subtotal { get; set; }
		public int? IdCompra { get; set; }
	}
}
