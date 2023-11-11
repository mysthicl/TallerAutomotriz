using Taller.Entity;

namespace Taller.AplicacionWeb.Models.ViewModels
{
    public class VMTblHistorialCarro
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
        public string? Status { get; set; }
        public string? usuario { get; set; }
        public string? NumberTracking { get; set; }


        public virtual ICollection<VMTblDetalleCotizacion> TblDetalleCotizacions { get; set; }


    }
}
