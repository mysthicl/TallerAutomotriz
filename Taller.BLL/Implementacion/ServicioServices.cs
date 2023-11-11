using Microsoft.EntityFrameworkCore;
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
    public class ServicioServices : IServicioServices
    {
        private readonly IGenericRepository<TblProducto> _repository;
        private readonly historyCarsContext _dbContext;
        private readonly IFireBaseServices _firebaseServices;


        public ServicioServices(IGenericRepository<TblProducto> repository,historyCarsContext dbcontext,IFireBaseServices firebaseServices) 
        {
            _repository = repository;
            _dbContext = dbcontext;
            _firebaseServices =firebaseServices;
          }



        public async Task<TblProducto> Crear(TblProducto producto ,Stream StreamFoto = null, string NombreFoto = "")
        {
            TblProducto productoExiste = await _repository.Obtener(p => p.CodigoProducto == producto.CodigoProducto);

            if (productoExiste != null)
            {
                throw new TaskCanceledException("El producto ya existe");
            }



            try
            {
                producto.NombreImagen = NombreFoto;


                if (StreamFoto != null)
                {
                    string UrlFoto = await _firebaseServices.SubirStorage(StreamFoto, "carpeta_producto", NombreFoto);
                    producto.UrlImagen = UrlFoto;
                }
                Correlativo numeroCorrelativo = _dbContext.Correlativos.Where(n => n.Tipo == "Servicio").First();

                numeroCorrelativo.NumeroCorrelativo = numeroCorrelativo.NumeroCorrelativo + 1;
                
                _dbContext.Correlativos.Update(numeroCorrelativo);
                await _dbContext.SaveChangesAsync();

                string ceros = string.Concat(Enumerable.Repeat("0", numeroCorrelativo.CantidadNumero.Value));
                string numeroVenta = ceros + numeroCorrelativo.NumeroCorrelativo.ToString();
                numeroVenta = numeroVenta.Substring(numeroVenta.Length - numeroCorrelativo.CantidadNumero.Value, numeroCorrelativo.CantidadNumero.Value);

                producto.CodigoProducto = numeroVenta;
                producto.Valor = "Servicio";
                producto.Ganancia =0;

                TblProducto productoCreate = await _repository.Crear(producto);

                if (string.IsNullOrEmpty(productoCreate.CodigoProducto))
                {
                    throw new TaskCanceledException("No se pudo registrar el producto");
                }


                return productoCreate;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<TblProducto> Editar(TblProducto producto)
        {
            TblProducto productoExiste = await _repository.Obtener(u => u.CodigoProducto == producto.CodigoProducto && u.IdProducto != producto.IdProducto);

            if (productoExiste != null)
            {
                throw new TaskCanceledException("El codigo de producto ya existe");
            }

            try
            {
                IQueryable<TblProducto> query = await _repository.Consultar(p => p.IdProducto == producto.IdProducto);
                TblProducto productoEdit = query.First();

                
                productoEdit.Nombre = producto.Nombre;
                productoEdit.Descripcion = producto.Descripcion;
                productoEdit.Precio = producto.Precio;
                

                
                bool resp = await _repository.Editar(productoEdit);

                if (!resp)
                    throw new TaskCanceledException("No se pudo modificar el servicio");

                TblProducto UsuarioEditado = query.First();

                return UsuarioEditado;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                TblProducto productoEncontrado = await _repository.Obtener(p => p.IdProducto == id);
                if (productoEncontrado == null)
                    throw new TaskCanceledException("El producto no existe");

                string nombreImagen = productoEncontrado.NombreImagen;
                bool res = await _repository.Eliminar(productoEncontrado);

               
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<TblProducto>> lista()
        {
            IQueryable<TblProducto> query = await _repository.Consultar(p=>p.Valor=="Servicio");

            return query.ToList();
        }
    }
}
