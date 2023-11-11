using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taller.DAL.Interfaz;
using Taller.Entity;

namespace Taller.DAL.Implementacion
{
    public class CompraRepository : GenericRepository<TblCompra>, ICompraRepository
    {
        private readonly historyCarsContext _dbContext;

        public CompraRepository(historyCarsContext dbcontext) : base(dbcontext)
        {
            _dbContext = dbcontext;
        }

        public async Task<TblCompra> RegistrarCompra (TblCompra entidad)
        {
            TblCompra CompraGenerada = new TblCompra();

            using (var Transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (TblDetalleCompra dv in entidad.TblDetalleCompras)
                    {
                        TblProducto producto_encontrado = _dbContext.TblProductos.Where(p => p.IdProducto == dv.IdProducto).First();
                            producto_encontrado.CantidadEnStock = producto_encontrado.CantidadEnStock + dv.Cantidad;
                            _dbContext.TblProductos.Update(producto_encontrado);
                    }

                    await _dbContext.SaveChangesAsync();

                    Correlativo numeroCorrelativo = _dbContext.Correlativos.Where(n => n.Tipo == "Compra").First();

                    numeroCorrelativo.NumeroCorrelativo = numeroCorrelativo.NumeroCorrelativo + 1;
                    _dbContext.Correlativos.Update(numeroCorrelativo);
                    await _dbContext.SaveChangesAsync();

                    string ceros = string.Concat(Enumerable.Repeat("0", numeroCorrelativo.CantidadNumero.Value));
                    string numeroVenta = ceros + numeroCorrelativo.NumeroCorrelativo.ToString();
                    numeroVenta = numeroVenta.Substring(numeroVenta.Length - numeroCorrelativo.CantidadNumero.Value, numeroCorrelativo.CantidadNumero.Value);

                    entidad.NumeroCompra = numeroVenta;
                    await _dbContext.TblCompras.AddAsync(entidad);
                    await _dbContext.SaveChangesAsync();

                    CompraGenerada = entidad;
                    Transaction.Commit();

                }
                catch (Exception ex)
                {

                    Transaction.Rollback();
                    
                }

            }
            return CompraGenerada;
        }
    }
}
