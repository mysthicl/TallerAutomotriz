using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taller.Entity;

namespace Taller.BLL.Interfaces
{
	public interface ICotizacionServices
	{

		Task<List<TblProducto>> ListarProductos(string busqueda);
		Task<TblCotizacion> Registrar(TblCotizacion cotizacion);
		Task<List<TblCotizacion>> Historial(string numeroCotizacion, string fechaInicio, string fechaFin);
		Task<TblCotizacion> Detalle(string numeroCotizacion);
		Task<string> ObtenerNumeroCotizacion();
	}
}
