using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taller.BLL.Interfaces;
using Taller.DAL.Interfaz;
using Taller.Entity;

namespace Taller.BLL.Implementacion
{
    public class DashBoardServices : IDashBoardServices
    {
        private readonly IVentaRepository _repositoryVenta;
        private readonly IGenericRepository<TblDetalleVenta> _repositoryDetallVenta;
        private readonly IGenericRepository<TblProducto> _repositoryProducto;
        private DateTime fechaInicio=DateTime.Now;

        public DashBoardServices(IVentaRepository repositoryVenta, IGenericRepository<TblDetalleVenta> repositoryDetallVenta,
            IGenericRepository<TblProducto> repositoryProducto)
        {
            _repositoryVenta= repositoryVenta;
            _repositoryDetallVenta = repositoryDetallVenta;
            _repositoryProducto= repositoryProducto;
            fechaInicio = fechaInicio.AddDays(-7);
        }



        public async Task<Dictionary<string, int>> ProductosTopUltimaSemana()
        {
            try
            {
                IQueryable<TblDetalleVenta> query = await _repositoryDetallVenta.Consultar();
                
                Dictionary<string, int> result = query
                    .Include(v=>v.IdVentaNavigation)
                    .Include(v=>v.IdProductoNavigation)
                    .Where(dv=>dv.IdVentaNavigation.Fecha.Value.Date>=fechaInicio.Date)
                    .GroupBy(dv=>dv.IdProductoNavigation.Descripcion).OrderByDescending(g => g.Count())
                    .Select(dv => new { producto = dv.Key, total = dv.Count() })
                    .Take(4)
                    .ToDictionary(keySelector: r => r.producto, elementSelector: r => r.total);

                return result;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async  Task<string> TotalIngresoUltimaSemana()
        {
            try
            {
                IQueryable<TblVenta> query = await _repositoryVenta.Consultar(v => v.Fecha.Value.Date >= fechaInicio.Date);
                decimal resul = query
                    .Select(v => v.TotalDeLaVenta)
                    .Sum(v => v.Value);
                return Convert.ToString(resul,new CultureInfo("es-SV"));

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> TotalProductos()
        {
            try
            {
                IQueryable<TblProducto> query = await _repositoryProducto.Consultar(p=>p.Valor!="Servicio");
                int total = query.Count();
                return total;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> TotalServicios()
        {
            try
            {
                IQueryable<TblProducto> query = await _repositoryProducto.Consultar(p => p.Valor == "Servicio");
                int total = query.Count();
                return total;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> TotalVentasUltimaSemana()
        {
            try
            {
                IQueryable<TblVenta> query = await _repositoryVenta.Consultar(v => v.Fecha.Value.Date >= fechaInicio.Date);
                int total=query.Count();
                return total;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Dictionary<string, int>> VentasUltimaSemana()
        {
            try
            {
                IQueryable<TblVenta> query = await _repositoryVenta.Consultar(v => v.Fecha.Value.Date >= fechaInicio.Date);
                Dictionary<string, int> result = query
                    .GroupBy(v => v.Fecha.Value.Date).OrderByDescending(g => g.Key)
                    .Select(dv => new { fecha = dv.Key.ToString("dd/MM/yyyy"), total = dv.Count() })
                    .ToDictionary(keySelector:r=>r.fecha,elementSelector:r=>r.total);

                return result;  
            
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
