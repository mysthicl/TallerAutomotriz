using Taller.Entity;

namespace Taller.AplicacionWeb.Models.ViewModels
{
	public class VMTblDetalleCotizacion
	{



		public int IdDetalleCotizacion { get; set; }
		public decimal? Precio { get; set; }
		public int? Cantidad { get; set; }
		public decimal? Subtotal { get; set; }
		public int? IdCotizacion { get; set; }
		public int? IdProducto { get; set; }

		public string? DescripcionProducto { get; set; }



	}
}
