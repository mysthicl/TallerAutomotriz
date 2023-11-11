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
    public class ProductoServices : IProductoServices
    {
        private readonly IGenericRepository<TblProducto> _repository;
        private readonly IFireBaseServices _firebaseServices;
        
        public ProductoServices(IGenericRepository<TblProducto> repository, IFireBaseServices firebaseServices)
        {
            _repository= repository;
            _firebaseServices= firebaseServices;
        }


        public async Task<TblProducto> Crear(TblProducto producto, Stream imagen = null, string nombreImagen = "")
        {
            TblProducto productoExiste = await _repository.Obtener(p => p.CodigoProducto == producto.CodigoProducto);

            if (productoExiste != null)
            {
                throw new TaskCanceledException("El producto ya existe");
            }


            try
            {

                producto.NombreImagen = nombreImagen;

                if (imagen != null)
                {
                    string urlImagen = await _firebaseServices.SubirStorage(imagen,"carpeta_producto", nombreImagen);
                    producto.UrlImagen=urlImagen;
                }

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

        public async Task<TblProducto> Editar(TblProducto producto, Stream imagen = null, string nombreImagen = "")
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

                productoEdit.CodigoProducto = producto.CodigoProducto;
                productoEdit.Nombre = producto.Nombre;
                productoEdit.Descripcion=producto.Descripcion;
                productoEdit.Ganancia = producto.Ganancia;
                productoEdit.Precio = producto.Precio;
                productoEdit.CantidadEnStock = producto.CantidadEnStock;

                if (imagen != null)
                {
                    string urlImagen = await _firebaseServices.SubirStorage(imagen,"carpeta_producto", productoEdit.NombreImagen);
                    productoEdit.UrlImagen=urlImagen;
                }
                bool resp = await _repository.Editar(productoEdit);

                if (!resp)
                    throw new TaskCanceledException("No se pudo modificar el usuario");

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
                if(productoEncontrado == null)
                    throw new TaskCanceledException("El producto no existe");

                string nombreImagen = productoEncontrado.NombreImagen;
                bool res = await _repository.Eliminar(productoEncontrado);

                if (res)
                {
                    await _firebaseServices.EliminarStorage("carpeta_producto",nombreImagen);
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<TblProducto>> Lista()
        {
            IQueryable<TblProducto> query = await _repository.Consultar();

            return query.ToList();
        }

        public async Task<List<TblProducto>> lista()
        {
            IQueryable<TblProducto> query = await _repository.Consultar(p=>p.Valor!="Servicio");

            return query.ToList();
        }
    }
}
