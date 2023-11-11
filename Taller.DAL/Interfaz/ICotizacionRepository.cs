using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taller.Entity;

namespace Taller.DAL.Interfaz
{
	public interface ICotizacionRepository : IGenericRepository<TblCotizacion>
	{
		Task<TblCotizacion> RegistrarCotizacion(TblCotizacion entidad);
	}
}
