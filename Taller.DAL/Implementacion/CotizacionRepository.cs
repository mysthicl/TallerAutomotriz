using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taller.DAL.Interfaz;
using Taller.Entity;

namespace Taller.DAL.Implementacion
{
	public class CotizacionRepository : GenericRepository<TblCotizacion>, ICotizacionRepository
	{
		private readonly historyCarsContext _dbContext;

		public CotizacionRepository(historyCarsContext dbcontext) : base(dbcontext)
		{
			_dbContext = dbcontext;
		}

		public async Task<TblCotizacion> RegistrarCotizacion(TblCotizacion entidad)
		{
			TblCotizacion cotizacionGenerada = new TblCotizacion();

			using (var Transaction = _dbContext.Database.BeginTransaction())
			{
				try
				{
					foreach (TblDetalleCotizacion dv in entidad.TblDetalleCotizacions)
					{
						TblProducto producto_encontrado = _dbContext.TblProductos.Where(p => p.IdProducto == dv.IdProducto).First();
						
					}
					await _dbContext.SaveChangesAsync();


					Correlativo numeroCorrelativo = _dbContext.Correlativos.Where(n => n.Tipo == "Cotizacion").First();

					numeroCorrelativo.NumeroCorrelativo = numeroCorrelativo.NumeroCorrelativo + 1;
					_dbContext.Correlativos.Update(numeroCorrelativo);
					await _dbContext.SaveChangesAsync();

					string ceros = string.Concat(Enumerable.Repeat("0", numeroCorrelativo.CantidadNumero.Value));
					string numeroVenta = ceros + numeroCorrelativo.NumeroCorrelativo.ToString();
					numeroVenta = numeroVenta.Substring(numeroVenta.Length - numeroCorrelativo.CantidadNumero.Value, numeroCorrelativo.CantidadNumero.Value);

					entidad.NumeroCotizacion = numeroVenta;
					await _dbContext.TblCotizacions.AddAsync(entidad);
					await _dbContext.SaveChangesAsync();

					cotizacionGenerada = entidad;
					Transaction.Commit();

				}
				catch (Exception ex)
				{

					Transaction.Rollback();

				}

			}
			return cotizacionGenerada;
		}
	}
}
