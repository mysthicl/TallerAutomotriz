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
	public class ClienteServices : IClienteServices
	{
		private readonly IGenericRepository<TblReparacion> _repository;

        public ClienteServices(IGenericRepository<TblReparacion> repository)
        {
			_repository = repository;
        }
        public async Task<TblReparacion> TrackingNumber(string tracking)
		{
			TblReparacion TrackingNumberEncontrado = await _repository.Obtener(c => c.NumberTracking.Equals(tracking));

			return TrackingNumberEncontrado;
		}
	}
}
