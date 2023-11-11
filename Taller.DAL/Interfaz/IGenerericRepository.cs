using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Taller.DAL.Interfaz
{
        public interface IGenericRepository<TEntity> where TEntity : class
        {
            Task<TEntity> Obtener(Expression<Func<TEntity, bool>> filtro);
            Task<TEntity> Crear(TEntity entidad);
            Task<bool> Editar(TEntity entidad);
            Task<bool> Eliminar(TEntity entidad);
            Task<IQueryable<TEntity>> Consultar(Expression<Func<TEntity, bool>> filtro = null, Expression<Func<TEntity, bool>> filtro2 = null);
        }
}
