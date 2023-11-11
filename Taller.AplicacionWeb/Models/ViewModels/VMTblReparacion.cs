using System.Security.Policy;

namespace Taller.AplicacionWeb.Models.ViewModels
{
	public class VMTblReparacion
	{
        public int IdHistorialCarro { get; set; }
        public int? IdReparacion { get; set; }
        public string? Placa { get; set; }
        public int? IdPlaca { get; set; }
        public string? Marca { get; set; }
        public string? DescripcionDeLaReparacion { get; set; }
        public DateTime? FechaDeInicio { get; set; }
        public DateTime? FechaDeFin { get; set; }
        public int? IdCotizacion { get; set; }
        public int? IdUsuario { get; set; }
        public string? Status { get; set; }
        public string? usuario { get; set; }
        public string? NumberTracking { get; set; }
        public int IdCarro { get; set; }
        public decimal subtotal { get; set; }
        public double iva { get; set; }
        public double Total { get; set; }

        public virtual ICollection<VMTblDetalleCotizacion> TblDetalleCotizacions { get; set; }

    }
}
