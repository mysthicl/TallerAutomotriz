using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taller.BLL.Interfaces;
using Taller.DAL.Implementacion;
using Taller.DAL.Interfaz;
using Taller.Entity;

namespace Taller.BLL.Implementacion
{
	public class CotizacionServices : ICotizacionServices
	{
		private readonly IGenericRepository<TblProducto> _repositoryProducto;
		private readonly ICotizacionRepository _cotizacionRepository;
		private readonly historyCarsContext _dbContext;
		public CotizacionServices(IGenericRepository<TblProducto> repositoryProducto, ICotizacionRepository cotizacionRepository, historyCarsContext dbContext)
		{
			_repositoryProducto = repositoryProducto;
			_cotizacionRepository = cotizacionRepository;
			_dbContext = dbContext;
		}

		public async Task<TblCotizacion> Detalle(string numeroCotizacion)
		{
			IQueryable<TblCotizacion> query = await _cotizacionRepository.Consultar(c => c.NumeroCotizacion == numeroCotizacion);

			return query
				   .Include(dv => dv.TblDetalleCotizacions)
				   .Include(u => u.IdUsuarioNavigation).First();
		}

		public async Task<List<TblCotizacion>> Historial(string numeroCotizacion, string fechaInicio, string fechaFin)
		{
			IQueryable<TblCotizacion> query = await _cotizacionRepository.Consultar();
			fechaInicio = fechaInicio is null ? "" : fechaInicio;
			fechaFin = fechaFin is null ? "" : fechaFin;

			if (fechaInicio != "" && fechaFin != "")
			{
				DateTime fech_inicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new CultureInfo("es-SV"));
				DateTime fech_fin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new CultureInfo("es-SV"));

				return query.Where(v => v.FechaCotizacion.Value.Date >= fech_inicio.Date &&
					v.FechaCotizacion.Value.Date <= fech_fin.Date)
						.Include(dv => dv.TblDetalleCotizacions).ThenInclude(dv => dv.IdProductoNavigation)
						.Include(u => u.IdUsuarioNavigation).ToList();

			}
			else
			{
				return query.Where(v => v.NumeroCotizacion == numeroCotizacion)
					 .Include(v => v.TblDetalleCotizacions)
						 .ThenInclude(dv => dv.IdProductoNavigation)
					 .Include(u => u.IdUsuarioNavigation)
					 .ToList();
			}
		}

		public async Task<List<TblProducto>> ListarProductos(string busqueda)
		{
			IQueryable<TblProducto> query = await _repositoryProducto.Consultar(
			 p => (p.CantidadEnStock > 0 || p.Valor == "Servicio") &&
			  string.Concat(p.CodigoProducto, p.Nombre, p.Descripcion).Contains(busqueda));

			return query.ToList();
		}

		public async Task<string> ObtenerNumeroCotizacion()
		{
			Correlativo numeroCorrelativo = _dbContext.Correlativos.Where(n => n.Tipo == "Cotizacion").First();

			numeroCorrelativo.NumeroCorrelativo = numeroCorrelativo.NumeroCorrelativo + 1;


			string ceros = string.Concat(Enumerable.Repeat("0", numeroCorrelativo.CantidadNumero.Value));
			string numeroCompra = ceros + numeroCorrelativo.NumeroCorrelativo.ToString();
			numeroCompra = numeroCompra.Substring(numeroCompra.Length - numeroCorrelativo.CantidadNumero.Value, numeroCorrelativo.CantidadNumero.Value);

			return numeroCompra;
		}

		public async Task<TblCotizacion> Registrar(TblCotizacion cotizacion)
		{
			try
			{
				return await _cotizacionRepository.RegistrarCotizacion(cotizacion);
			}
			catch (Exception)
			{

				throw;
			}
		}
	}
}
