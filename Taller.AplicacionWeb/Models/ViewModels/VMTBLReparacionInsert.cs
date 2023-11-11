namespace Taller.AplicacionWeb.Models.ViewModels
{
    public class VMTBLReparacionInsert
    {

        public int? IdReparacion { get; set; }
        public string? Status { get; set; }
        public string? NumberTracking { get; set; }
        public int? IdUsuario { get; set; }
        public string? DescripcionDeLaReparacion { get; set; }
        public string FechaDeInicio { get; set; }
        public int IdCarro { get; set; }
        public int? IdCotizacion { get; set; }
        public string hora { get; set; }
        public int? IdHistorialCarro { get; set; }

    }
}
