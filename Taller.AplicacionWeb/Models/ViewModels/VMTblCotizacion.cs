using Taller.Entity;

namespace Taller.AplicacionWeb.Models.ViewModels
{
	public class VMTblCotizacion
	{

		public int IdCotizacion { get; set; }
		public string? NumeroCotizacion { get; set; }
		public int? IdUsuario { get; set; }
		public string? Usuario { get; set; }
		public string? TotalDeLaCotizacion { get; set; }
		public string? FechaCotizacion { get; set; }
		public decimal? subtotal { get; set; }
		public double? iva { get; set; }
	

		public virtual ICollection<VMTblDetalleCotizacion> TblDetalleCotizacions { get; set; }
	}
}
