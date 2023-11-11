using Microsoft.CodeAnalysis.RulesetToEditorconfig;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
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
    public class CompraServices : ICompraServices
    {
        private readonly IGenericRepository<TblProducto> _repositoryProducto;
        private readonly ICompraRepository _compraRepository;
        private readonly historyCarsContext _dbContext;
        public CompraServices(IGenericRepository<TblProducto> repositoryProducto, ICompraRepository compraRepository, historyCarsContext dbContext)
        {
            _repositoryProducto = repositoryProducto;
            _compraRepository = compraRepository;
            _dbContext = dbContext;
        }

        public async Task<TblCompra> Detalle(string numeroCompra)
        {
            IQueryable<TblCompra> query = await _compraRepository.Consultar(c => c.NumeroCompra == numeroCompra);

            return query
                   .Include(dv => dv.TblDetalleCompras)
                   .Include(u => u.IdUsuarioNavigation).First();
        }

        public async Task<List<TblCompra>> Historial(string numeroCompra, string fechaInicio, string fechaFin)
        {
            IQueryable<TblCompra> query = await _compraRepository.Consultar();
            fechaInicio = fechaInicio is null ? "" : fechaInicio;
            fechaFin = fechaFin is null ? "" : fechaFin;

            if (fechaInicio != "" && fechaFin != "")
            {
                DateTime fech_inicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new CultureInfo("es-SV"));
                DateTime fech_fin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new CultureInfo("es-SV"));

                return query.Where(v => v.Fecha.Value.Date >= fech_inicio.Date &&
                    v.Fecha.Value.Date <= fech_fin.Date)
                        .Include(dv => dv.TblDetalleCompras).ThenInclude(dv => dv.IdProductoNavigation)
                        .Include(u => u.IdUsuarioNavigation).ToList();

            }
            else
            {
                return query.Where(v => v.NumeroCompra == numeroCompra)
                     .Include(v => v.TblDetalleCompras)
                         .ThenInclude(dv => dv.IdProductoNavigation)
                     .Include(u => u.IdUsuarioNavigation)
                     .ToList();
            }
        }

        public async Task<List<TblProducto>> ListarProductos(string busqueda)
        {
            IQueryable<TblProducto> query = await _repositoryProducto.Consultar(
                p => (p.CantidadEnStock > 0 || p.Valor != "Servicio") &&
                 string.Concat(p.CodigoProducto, p.Nombre, p.Descripcion).Contains(busqueda));

            return query.ToList();
        }

        public async Task<string> ObtenerNumeroCompra()
        {
            Correlativo numeroCorrelativo = _dbContext.Correlativos.Where(n => n.Tipo == "Compra").First();

            numeroCorrelativo.NumeroCorrelativo = numeroCorrelativo.NumeroCorrelativo + 1;


            string ceros = string.Concat(Enumerable.Repeat("0", numeroCorrelativo.CantidadNumero.Value));
            string numeroCompra = ceros + numeroCorrelativo.NumeroCorrelativo.ToString();
            numeroCompra = numeroCompra.Substring(numeroCompra.Length - numeroCorrelativo.CantidadNumero.Value, numeroCorrelativo.CantidadNumero.Value);

            return numeroCompra;
        }

        public async Task<TblCompra> Registrar(TblCompra compra)
        {
            try
            {
                return await _compraRepository.RegistrarCompra(compra);
            }
            catch (Exception)
            {

                throw;
            }
        }

        


     
    }
}
