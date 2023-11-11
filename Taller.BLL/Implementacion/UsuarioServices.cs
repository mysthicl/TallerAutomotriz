using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Taller.BLL.Interfaces;
using Taller.Entity;
using Taller.DAL.Interfaz;
using System.Net.Http;
using System.Security.Claims;

namespace Taller.BLL.Implementacion
{
   
    public class UsuarioServices : IUsuarioServices
    {
        private readonly IGenericRepository<TblUsuario> _repository;
        private readonly IFireBaseServices _firebaseServices;
        
        

        public UsuarioServices(IGenericRepository<TblUsuario> repository, IFireBaseServices firebaseServices)
        {
            _repository = repository;
            _firebaseServices = firebaseServices;
            

        }

		public async Task<TblUsuario> Crear(TblUsuario entidad, Stream StreamFoto = null, string NombreFoto = "")
		{
           
            TblUsuario ExisteUsuario = await _repository.Obtener(u => u.NombreDeUsuario == entidad.NombreDeUsuario);
			if (ExisteUsuario != null)
			{
				throw new TaskCanceledException("El usuario ya existe");
			}

			try
			{


                entidad.NombreFoto = NombreFoto;
                
                
                if (StreamFoto != null)
                {
                    string UrlFoto = await _firebaseServices.SubirStorage(StreamFoto, "carpeta_usuario", NombreFoto);
                    entidad.UrlFoto = UrlFoto;
                }
                
                
                TblUsuario UserCreate = await _repository.Crear(entidad);

                if (UserCreate.IdUsuario == 0)
                {
                    throw new TaskCanceledException("No se pudo crear el usuario");
                }

                IQueryable<TblUsuario> query = await _repository.Consultar(u => u.IdUsuario == UserCreate.IdUsuario);
                UserCreate = query.Include(r => r.IdRolNavigation).First();

                return UserCreate;

            }
			catch (Exception ex)
			{

				if (ex.InnerException != null)
				{
					string mensajeError = "Error: " + ex.Message + ". Excepción interna: " + ex.InnerException.Message;
					throw new Exception(mensajeError);
				}
				else
				{
					string mensajeError = "Error fiu: " + ex.Message;
					throw new Exception(mensajeError);
				}
			}
		}

		public async Task<TblUsuario> Editar(TblUsuario entidad, Stream StreamFoto = null, string NombreFoto = "")
		{

            TblUsuario ExisteUsuario = await _repository.Obtener(u => u.NombreDeUsuario == entidad.NombreDeUsuario && u.IdUsuario != entidad.IdUsuario);

            if (ExisteUsuario != null)
            {
                throw new TaskCanceledException("El usuario ya existe");
            }

            try
			{
				IQueryable<TblUsuario> queryUser = await _repository.Consultar(u => u.IdUsuario == entidad.IdUsuario);

				TblUsuario UsuarioEditar = queryUser.First();
				UsuarioEditar.NombreDeUsuario = entidad.NombreDeUsuario;
				UsuarioEditar.Contrasena = entidad.Contrasena;
				UsuarioEditar.FechaRegistro = entidad.FechaRegistro;
                UsuarioEditar.IdRol=entidad.IdRol;
				
				if (UsuarioEditar.NombreFoto == "")
					UsuarioEditar.NombreFoto = NombreFoto;

				if (StreamFoto != null)
				{
					string UrlFoto = await _firebaseServices.SubirStorage(StreamFoto, "carpeta_usuario", UsuarioEditar.NombreFoto);
					UsuarioEditar.UrlFoto = UrlFoto;
				}

                bool resp = await _repository.Editar(UsuarioEditar);

                if (!resp)
                    throw new TaskCanceledException("No se pudo modificar el usuario");

                TblUsuario UsuarioEditado = queryUser.Include(r => r.IdRolNavigation).First();

                return UsuarioEditado;
            }
			catch (Exception)
			{

				throw;
			}
		}

		public async Task<List<TblUsuario>> Lista(int idUsuario)
        {

            
            IQueryable<TblUsuario> query = await _repository.Consultar(u=>u.IdUsuario!= 39, u=>u.IdUsuario!=idUsuario);
            return query.Include(r => r.IdRolNavigation).ToList();

        }

        

        public async Task<TblUsuario> ObtenerPorCredenciales(string nombre, string clave)
        {
            
            TblUsuario usuarioEncontrado = await _repository.Obtener(u => u.NombreDeUsuario.Equals(nombre) && u.Contrasena.Equals(clave));

            return usuarioEncontrado;
        }

        public async Task<bool> Eliminar(int idUsuario)
        {
            try
            {
                TblUsuario UsuarioEncontrado = await _repository.Obtener(u => u.IdUsuario == idUsuario);
                
                if (UsuarioEncontrado == null)
                {
                    throw new TaskCanceledException("El usuario no existe");
                }

                string nombreFoto = UsuarioEncontrado.NombreFoto;
                bool result = await _repository.Eliminar(UsuarioEncontrado);
                if (result)
                {
                    await _firebaseServices.EliminarStorage("carpeta_usuario", nombreFoto);
                }

                return true;

            }
            catch (Exception)
            {
            
                throw;
            }
        }




    }
}
