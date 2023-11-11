using System.Security.Policy;

namespace Taller.AplicacionWeb.Models.ViewModels
{
    public class VMTblVenta
    {

        public int IdVenta { get; set; }
        public string? NumeroVenta { get; set; }
        public int? IdUsuario { get; set; }
        public string? Usuario { get; set; }
        public string? DocumentoCliente { get; set; }
        public string? NombreCliente { get; set; }
        public int? Telefono { get; set; }
        public string? Direccion { get; set; }
        public string? TotalDeLaVenta { get; set; }
        public string? Fecha { get; set; }
        public decimal? subtotal { get; set; }

        public double? iva { get; set; }


        public virtual ICollection<VMTblDetalleVenta> TblDetalleVenta { get; set; }
    }
}
