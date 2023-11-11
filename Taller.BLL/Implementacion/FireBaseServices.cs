using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taller.BLL.Interfaces;
using Taller.DAL.Interfaz;
using Firebase.Auth;
using Firebase.Storage;
using Taller.Entity;

namespace Taller.BLL.Implementacion
{
    public class FireBaseServices : IFireBaseServices
    {
        private readonly IGenericRepository<Configuracion> _repository;

        public FireBaseServices(IGenericRepository<Configuracion> repository)
        {
            _repository = repository;
        }

        public async Task<string> SubirStorage(Stream streamArchivo, string CarpetaDestino, string NombreArchivo)
        {
            string UrlImagen = "";
            try
            {
                IQueryable<Configuracion> query = await _repository.Consultar(c => c.Recurso.Equals("FireBase_Storage"));
                Dictionary<string, string> Config = query.ToDictionary(keySelector: c => c.Propiedad, elementSelector: c => c.Valor);
                var auth = new FirebaseAuthProvider(new FirebaseConfig(Config["api_key"]));
                var sign = await auth.SignInWithEmailAndPasswordAsync(Config["email"], Config["clave"]);

                var cancelacion = new CancellationTokenSource();
                var task = new FirebaseStorage(
                    Config["ruta"],
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(sign.FirebaseToken),
                        ThrowOnCancel = true
                    }).Child(Config[CarpetaDestino])
                    .Child(NombreArchivo)
                    .PutAsync(streamArchivo, cancelacion.Token);

                UrlImagen = await task;



            }
            catch (Exception ex)
            {
                string mensajeError = "error: " + ex.Message;
                throw new Exception(mensajeError);
            }
            return UrlImagen;
        }
        public async Task<bool> EliminarStorage(string CarpetaDestino, string NombreArchivo)
        {
            try
            {
                IQueryable<Configuracion> query = await _repository.Consultar(c => c.Recurso.Equals("FireBase_Storage"));
                Dictionary<string, string> Config = query.ToDictionary(keySelector: c => c.Propiedad, elementSelector: c => c.Valor);

                var auth = new FirebaseAuthProvider(new FirebaseConfig(Config["api_key"]));
                var sign = await auth.SignInWithEmailAndPasswordAsync(Config["email"], Config["clave"]);

                var cancelacion = new CancellationTokenSource();
                var task = new FirebaseStorage(
                    Config["ruta"],
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(sign.FirebaseToken),
                        ThrowOnCancel = true
                    }).Child(Config[CarpetaDestino])
                    .Child(NombreArchivo)
                    .DeleteAsync();

                await task;

                return true;

            }
            catch
            {
                return false;
            }
        }


    }
}
