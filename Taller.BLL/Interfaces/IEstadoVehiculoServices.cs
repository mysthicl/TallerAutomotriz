using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taller.Entity;

namespace Taller.BLL.Interfaces
{
	public interface IEstadoVehiculoServices
	{
		Task<TblReparacion> Obtener(string NumberTracking);
	}
}
