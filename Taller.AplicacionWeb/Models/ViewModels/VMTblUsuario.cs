namespace Taller.AplicacionWeb.Models.ViewModels
{
    public class VMTblUsuario
    {
        public int IdUsuario { get; set; }
        public string? NombreDeUsuario { get; set; }
        public string? Contrasena { get; set; }
        public string? nombreRol { get; set; }
        public string? UrlFoto { get; set; }
        public int? IdRol { get; set; }
        public DateTime? FechaRegistro { get; set; }

	}
}
