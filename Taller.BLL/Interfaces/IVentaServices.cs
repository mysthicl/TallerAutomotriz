using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taller.Entity;

namespace Taller.BLL.Interfaces
{
	public interface IVentaServices
	{
		Task<List<TblProducto>> ListarProductos(string busqueda);
		Task<TblVenta> Registrar(TblVenta venta);
		Task<List<TblVenta>> Historial(string numeroVenta, string fechaInicio, string fechaFin);
		Task<TblVenta> Detalle(string numeroVenta);
		Task<List<TblDetalleVenta>> Reporte(string fechaInicio, string fechaFin);

		Task<string> ObtenerNumeroVenta();
	}
}
