using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Taller.DAL.Interfaz;
using Taller.Entity;

namespace Taller.DAL.Implementacion
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly historyCarsContext _dbcontext;

        public GenericRepository(historyCarsContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<TEntity> Obtener(Expression<Func<TEntity, bool>> filtro)
        {
            try
            {
                TEntity entidad = await _dbcontext.Set<TEntity>().FirstOrDefaultAsync(filtro);
                return entidad;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<TEntity> Crear(TEntity entidad)
        {
            try
            {
                _dbcontext.Set<TEntity>().Add(entidad);
                await _dbcontext.SaveChangesAsync();
                return entidad;

            }
            catch (Exception ex)
            {

                string mensajeError = $"Ocurrió un error al crear la entidad {typeof(TEntity).Name}. Detalles del error: {ex.InnerException?.Message ?? ex.Message}";
                throw new Exception(mensajeError);
            }
        }

        public async Task<bool> Editar(TEntity entidad)
        {
            try
            {
                _dbcontext.Set<TEntity>().Update(entidad);
                await _dbcontext.SaveChangesAsync();
                return true;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> Eliminar(TEntity entidad)
        {
            try
            {
                _dbcontext.Set<TEntity>().Remove(entidad);
                await _dbcontext.SaveChangesAsync();
                return true;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IQueryable<TEntity>> Consultar(Expression<Func<TEntity, bool>> filtro = null, Expression<Func<TEntity, bool>> filtro2 = null)
        {
            IQueryable<TEntity> queryEntidad = _dbcontext.Set<TEntity>();

            if (filtro != null)
            {
                queryEntidad = queryEntidad.Where(filtro);
            }

            if (filtro2 != null)
            {
                queryEntidad = queryEntidad.Where(filtro2);
            }

            return queryEntidad;
        }
    }
}
