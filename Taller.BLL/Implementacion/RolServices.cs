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
    public class RolServices : IRolServices
    {
        private readonly IGenericRepository<TblRol> _repository;
        public RolServices(IGenericRepository<TblRol> repository)
        {
            _repository = repository;
        }

        public async Task<List<TblRol>> Lista()
        {
            IQueryable<TblRol> query = await _repository.Consultar();
            return query.ToList();
        }
    }
}
