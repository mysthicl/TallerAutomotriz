using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Taller.DAL.Interfaz;
using Taller.Entity;

namespace Taller.DAL.Implementacion
{
    public class VentaRepository : GenericRepository<TblVenta>, IVentaRepository
    {
        private readonly historyCarsContext _dbContext;

        public VentaRepository(historyCarsContext dbcontext) : base(dbcontext)
        {
            _dbContext = dbcontext;
        }

        public async Task<TblVenta> RegistrarVenta(TblVenta entidad)
        {
            TblVenta VentaGenerada = new TblVenta();

            using (var Transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (TblDetalleVenta dv in entidad.TblDetalleVenta)
                    {
                        TblProducto producto_encontrado = _dbContext.TblProductos.Where(p => p.IdProducto == dv.IdProducto).First();
                        if (producto_encontrado.Valor != "Servicio")
                        {
							producto_encontrado.CantidadEnStock = producto_encontrado.CantidadEnStock - dv.Cantidad;
							_dbContext.TblProductos.Update(producto_encontrado);
						}

						
					}
                    await _dbContext.SaveChangesAsync();

                    Correlativo numeroCorrelativo = _dbContext.Correlativos.Where(n => n.Tipo == "venta").First();

                    numeroCorrelativo.NumeroCorrelativo = numeroCorrelativo.NumeroCorrelativo + 1;
                    _dbContext.Correlativos.Update(numeroCorrelativo);
                    await _dbContext.SaveChangesAsync();

                    string ceros = string.Concat(Enumerable.Repeat("0", numeroCorrelativo.CantidadNumero.Value));
                    string numeroVenta = ceros + numeroCorrelativo.NumeroCorrelativo.ToString();
                    numeroVenta = numeroVenta.Substring(numeroVenta.Length - numeroCorrelativo.CantidadNumero.Value,numeroCorrelativo.CantidadNumero.Value);

                    entidad.NumeroVenta = numeroVenta;
                    await _dbContext.TblVenta.AddAsync(entidad);
                    await _dbContext.SaveChangesAsync();

                    VentaGenerada = entidad;
                    Transaction.Commit();

                }
                catch (Exception ex)
                {

                    Transaction.Rollback();
                    throw ex;
                }

            }
            return VentaGenerada;
        }

        public async Task<List<TblDetalleVenta>> ReporteVenta(DateTime FechaInicio, DateTime FechaFin)
        {
            List<TblDetalleVenta> ListaReporte = await _dbContext.TblDetalleVenta
            .Include(v => v.IdVentaNavigation)
            .ThenInclude(u => u.IdUsuarioNavigation)
            .Include(v => v.IdVentaNavigation)
            .Where(dv => dv.IdVentaNavigation.Fecha.Value.Date >= FechaInicio.Date &&
                dv.IdVentaNavigation.Fecha.Value.Date <= FechaFin.Date).ToListAsync();

            return ListaReporte;
        }
    }
}
