using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
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
	public class ReparacionServices : IReparacionServices
	{
		private readonly IGenericRepository<TblReparacion> _repository;
		private readonly IGenericRepository<TblCotizacion> _repositoryCotizacion;
		private readonly IGenericRepository<TblCarro> _repositoryCarro;
		private readonly IGenericRepository<TblHistorialCarro> _repositoryHistorialCarro;
		private readonly historyCarsContext _dbContext;

        public ReparacionServices(IGenericRepository<TblReparacion> repository, IGenericRepository<TblCotizacion> repositoryCotizacion,
			IGenericRepository<TblCarro> repositoryCarro, IGenericRepository<TblHistorialCarro> repositoryHistorialCarro, historyCarsContext dbContext)
		{
			_repository = repository;
			_repositoryCotizacion = repositoryCotizacion;
			_repositoryCarro = repositoryCarro;
			_repositoryHistorialCarro= repositoryHistorialCarro;
			_dbContext=dbContext;
		}

		public async Task<TblReparacion> Crear(TblReparacion entidad, int idCarro)
		{
			string number = Guid.NewGuid().ToString("N").Substring(0, 9);
			entidad.NumberTracking = number;

			

			try
			{
				TblHistorialCarro carrito = new TblHistorialCarro();
			
				carrito.IdCarro= idCarro;
				entidad.Status = "Revision";

                TblHistorialCarro HistorialCarro = await _repositoryHistorialCarro.Crear(carrito);
				entidad.IdHistorialCarro = HistorialCarro.IdHistorialCarro;
                TblReparacion UserCreate = await _repository.Crear(entidad);

                if (UserCreate.IdReparacion == 0)
				{
					throw new TaskCanceledException("No se pudo crear la reparacion");
				}

				IQueryable<TblReparacion> query = await _repository.Consultar(u => u.IdReparacion == UserCreate.IdReparacion);
				UserCreate = query.Include(r => r.IdCotizacionNavigation)
					.Include(hc=>hc.IdHistorialCarroNavigation)
					.ThenInclude(c=>c.IdCarroNavigation)
								  .Include(r => r.IdUsuarioNavigation)
                                  
                                  .First();


				return UserCreate;

			}
			catch (Exception ex)
			{

				if (ex.InnerException != null)
				{
					string mensajeError = "Error: " + ex.Message + ". Excepción interna: " + ex.InnerException.Message;
					throw new Exception(mensajeError);
				}
				else
				{
					string mensajeError = "Error fiu: " + ex.Message;
					throw new Exception(mensajeError);
				}
			}
		}

        public async Task ActualizarStatusReparacion(int idReparacion, string nuevoStatus)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    
                    var query = $"UPDATE tbl_reparacion SET status = '{nuevoStatus}' WHERE id_reparacion = {idReparacion}";
                    await _dbContext.Database.ExecuteSqlRawAsync(query);

                   
                    await _dbContext.SaveChangesAsync();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                   
                }
            }
        }

        public async Task<bool> Eliminar(int idHistorialCarro)
		{
			try
			{
				TblReparacion UsuarioEncontrado = await _repository.Obtener(u => u.IdReparacion == idHistorialCarro);

				if (UsuarioEncontrado == null)
				{
					throw new TaskCanceledException("La reparacion no existe");
				}
				if (UsuarioEncontrado.IdUsuarioNavigation.IdUsuario == 1)
				{
					bool result = await _repository.Eliminar(UsuarioEncontrado);

					return true;
				}

				return false;


			}
			catch (Exception)
			{

				throw;
			}
		}

        public async Task<List<TblReparacion>> HistorialVehiculo()
        {
            IQueryable<TblReparacion> query = await _repository.Consultar();
            return query.Include(r => r.IdHistorialCarroNavigation)
                .ThenInclude(hc => hc.IdCarroNavigation) 
                .Include(r => r.IdUsuarioNavigation) 
                .Include(r => r.IdCotizacionNavigation)
                    .ThenInclude(dc => dc.TblDetalleCotizacions)
                        .ThenInclude(p => p.IdProductoNavigation)
                .ToList();
        }


        public async Task<List<TblCotizacion>> ListarCotizacion(string busqueda)
		{
			IQueryable<TblCotizacion> query = await _repositoryCotizacion.Consultar(c => c.IdCotizacion > 0 && c.NumeroCotizacion.Contains(busqueda));

            List<TblCotizacion> cotizaciones = query.Include(c => c.TblDetalleCotizacions).ThenInclude(p=>p.IdProductoNavigation).ToList();
            return cotizaciones;
		}


		public async Task<List<TblCarro>> ListarVehiculo(string busqueda)
		{
			IQueryable<TblCarro> query = await _repositoryCarro.Consultar(
				c => c.IdCarro > 0  &&
				 string.Concat(c.Placa, c.Marca).Contains(busqueda));

			return query.ToList();
		}

        public async Task<bool> EliminarHistorialVehiculo(int idHistorialCarro)
        {
            try
            {
                TblReparacion UsuarioEncontrado = await _repository.Obtener(u => u.IdReparacion == idHistorialCarro);

                if (UsuarioEncontrado == null)
                {
                    throw new TaskCanceledException("La reparacion no existe");
                }
                
                    bool result = await _repository.Eliminar(UsuarioEncontrado);

                

                return true;


            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
