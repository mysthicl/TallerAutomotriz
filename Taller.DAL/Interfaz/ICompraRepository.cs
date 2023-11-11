using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taller.Entity;

namespace Taller.DAL.Interfaz
{
    public interface ICompraRepository : IGenericRepository<TblCompra>
    {
        Task<TblCompra> RegistrarCompra(TblCompra entidad);
    }
}
