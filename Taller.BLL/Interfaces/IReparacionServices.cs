using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taller.Entity;

namespace Taller.BLL.Interfaces
{
	public interface IReparacionServices
	{

		Task<List<TblCarro>> ListarVehiculo(string busqueda);

		Task<List<TblCotizacion>> ListarCotizacion(string busqueda);
		Task<TblReparacion> Crear(TblReparacion entidad, int idCarro);

		Task<List<TblReparacion>> HistorialVehiculo();

		Task ActualizarStatusReparacion(int idReparacion, string nuevoStatus);
        Task<bool> Eliminar(int idHistorialCarro);
        Task<bool> EliminarHistorialVehiculo(int idHistorialCarro);

    }
}
