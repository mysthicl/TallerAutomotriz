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
	public class VentaServices : IVentaServices
	{
		private readonly IGenericRepository<TblProducto> _repositoryProducto;
		private readonly IVentaRepository _ventaRepository;
		private readonly historyCarsContext _dbContext;
        public VentaServices(IGenericRepository<TblProducto> repositoryProducto, IVentaRepository ventaRepository, historyCarsContext dbContext)
        {
			_repositoryProducto = repositoryProducto;
			_ventaRepository = ventaRepository;
			_dbContext = dbContext;
        }

        public async Task<List<TblProducto>> ListarProductos(string busqueda)
        {
            IQueryable<TblProducto> query = await _repositoryProducto.Consultar(
               p => (p.CantidadEnStock > 0 || p.Valor == "Servicio") &&
				 string.Concat(p.CodigoProducto, p.Nombre, p.Descripcion).Contains(busqueda));

            return query.ToList();
        }

        public async Task<TblVenta> Registrar(TblVenta venta)
        {
			try
			{
				return await _ventaRepository.RegistrarVenta(venta);
			}
			catch (Exception)
			{

				throw;
			}
        }

        public async Task<List<TblVenta>> Historial(string numeroVenta, string fechaInicio, string fechaFin)
        {

            IQueryable<TblVenta> query = await _ventaRepository.Consultar();
			fechaInicio = fechaInicio is null ? "" : fechaInicio;
            fechaFin = fechaFin is null ? "" : fechaFin;

			if(fechaInicio!="" && fechaFin != "")
			{
				DateTime fech_inicio = DateTime.ParseExact(fechaInicio,"dd/MM/yyyy",new CultureInfo("es-SV"));
                DateTime fech_fin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new CultureInfo("es-SV"));

			return	query.Where(v => v.Fecha.Value.Date >= fech_inicio.Date &&
				v.Fecha.Value.Date <= fech_fin.Date)
					.Include(dv=>dv.TblDetalleVenta).ThenInclude(dv => dv.IdProductoNavigation)
                    .Include(u=>u.IdUsuarioNavigation).ToList();

			}
			else
			{
                return query.Where(v => v.NumeroVenta == numeroVenta)
                     .Include(v => v.TblDetalleVenta)
                         .ThenInclude(dv => dv.IdProductoNavigation)
                     .Include(u => u.IdUsuarioNavigation)
                     .ToList();
            }
            
        }


        public async Task<TblVenta> Detalle(string numeroVenta)
		{
            IQueryable<TblVenta> query = await _ventaRepository.Consultar(v=>v.NumeroVenta==numeroVenta);

           return query
                  .Include(dv => dv.TblDetalleVenta)
                  .Include(u => u.IdUsuarioNavigation).First();


        }

		public async Task<List<TblDetalleVenta>> Reporte(string fechaInicio, string fechaFin)
		{
            DateTime fech_inicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new CultureInfo("es-SV"));
            DateTime fech_fin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new CultureInfo("es-SV"));

            List<TblDetalleVenta> lista = await _ventaRepository.ReporteVenta(fech_inicio,fech_fin);

            return lista;

        }

		public async Task<string> ObtenerNumeroVenta()
		{
			Correlativo numeroCorrelativo = _dbContext.Correlativos.Where(n => n.Tipo == "venta").First();

			numeroCorrelativo.NumeroCorrelativo = numeroCorrelativo.NumeroCorrelativo + 1;
			

			string ceros = string.Concat(Enumerable.Repeat("0", numeroCorrelativo.CantidadNumero.Value));
			string numeroVenta = ceros + numeroCorrelativo.NumeroCorrelativo.ToString();
			numeroVenta = numeroVenta.Substring(numeroVenta.Length - numeroCorrelativo.CantidadNumero.Value, numeroCorrelativo.CantidadNumero.Value);

			return numeroVenta;
		}
	}
}
