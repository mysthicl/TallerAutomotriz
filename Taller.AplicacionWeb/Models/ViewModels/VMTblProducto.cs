namespace Taller.AplicacionWeb.Models.ViewModels
{
    public class VMTblProducto
    {
        public int IdProducto { get; set; }
        public string? CodigoProducto { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public string? Precio { get; set; }
        public int? CantidadEnStock { get; set; }
        public string? UrlImagen { get; set; }
		public string? Valor { get; set; }
        public string? Ganancia { get; set; }

    }
}
