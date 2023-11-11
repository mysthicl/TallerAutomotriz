using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Taller.Entity;

namespace Taller.BLL.Interfaces
{
    public interface IUsuarioServices
    {
        Task<List<TblUsuario>> Lista(int idUsuario);
        Task<TblUsuario> ObtenerPorCredenciales(string nombre, string clave);
		Task<TblUsuario> Crear(TblUsuario entidad, Stream StreamFoto = null, string NombreFoto = "");
		Task<TblUsuario> Editar(TblUsuario entidad, Stream StreamFoto = null, string NombreFoto = "");
        Task<bool> Eliminar(int idUsuario);
    }
}
