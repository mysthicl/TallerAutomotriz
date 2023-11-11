using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taller.BLL.Interfaces;
using Taller.DAL.Interfaz;
using Taller.Entity;

namespace Taller.BLL.Implementacion
{
	public class EstadoVehiculoServices : IEstadoVehiculoServices
	{
		private readonly IGenericRepository<TblReparacion> _repository;

        public EstadoVehiculoServices(IGenericRepository<TblReparacion> repository)
        {
			_repository = repository;
        }
        public async Task<TblReparacion> Obtener(string numberTracking)
		{
			TblReparacion obtenido = await _repository.Obtener(o => o.NumberTracking.Equals(numberTracking));

			return obtenido;
		}
	}
}
