using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taller.DAL.Interfaz;
using Taller.Entity;

namespace Taller.DAL.Interfaz
{
    public interface IVentaRepository : IGenericRepository <TblVenta>
    {
        Task<TblVenta> RegistrarVenta(TblVenta entidad);
        Task<List<TblDetalleVenta>> ReporteVenta(DateTime FechaInicio,DateTime FechaFin);

    }
}
