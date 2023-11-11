using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taller.Entity;

namespace Taller.BLL.Interfaces
{
    public interface ICompraServices
    {
        Task<List<TblProducto>> ListarProductos(string busqueda);
        Task<TblCompra> Registrar(TblCompra compra);
        Task<List<TblCompra>> Historial(string numeroCompra, string fechaInicio, string fechaFin);
        Task<TblCompra> Detalle(string numeroCompra);
        Task<string> ObtenerNumeroCompra();
    }
}
